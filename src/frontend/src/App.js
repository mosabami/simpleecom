// App.js
import React, { useState, useEffect } from 'react';
import {  Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import './App.css';
import ProductList from './components/Products/ProductList';
import Navbar from './components/Navbar/Navbar';
import ProductDetails from './components/Products/ProductDetails';
import Cart from "./components/Cart/Cart";
import LoginPage from './components/LoginRegistration/LoginPage';
import RegisterPage from './components/LoginRegistration/RegisterPage';
import placeholderCart from './placeholderCart.json';
let base_url = process.env.REACT_APP_API_BASE_URL || 'http://localhost:8083';
// If base_url is 'none', set it to an empty string. workaround to make it work with passing env variables from docker-compose
base_url = base_url === 'none' ? '' : base_url; 
let pull_brand_from_database = process.env.REACT_APP_PULL_BRAND_FROM_DATABASE || 'true';
console.log(`base_url: ${base_url}`);
console.log(`pull_brand_from_database: ${pull_brand_from_database}`);


const App = () =>  {
  const [loggedIn, setLoggedIn] = useState(false);
  const [wrongEmail, setwrongEmail] = useState(false);
  const [cart, setCart] = useState(false);
  const [userID, setUserID] = useState("");
  const [products, setProducts] = useState([]);
  const [user, setUserData] = useState({});
  const [brand, setBrand] = useState('all');
  const [brandData, setBrandData] = useState([]);
  
  // use this to navigate to brand
  const navigate = useNavigate();
  const handleBrandLinkClick = (brandName) => {
    setBrand(brandName);
    navigate(`/brand/${encodeURIComponent(brandName)}`);
    // navigate(`/brand`);
  };


  const handleLogin = async (email) => {
    let login_endpoint = `${base_url}/api/Auth/Login?email=${encodeURIComponent(email)}`;
    console.log(`login_endpoint: ${login_endpoint}`);

    try {
      let response = await fetch(login_endpoint);
      if (!response.ok) {
        setwrongEmail(true);
        throw new Error('Email not registered');
      }
      let userData = await response.json();
      if (!userData) {
        throw new Error('userData is undefined');
      }
      setLoggedIn(true);
      setUserID(userData["id"]);
      setwrongEmail(false);
      setUserData(userData);
      // console.log('userData:', userData);
      let productResponse = await fetch(`${base_url}/api/Product/GetProducts`)
        .then(response => response.json())
        .catch((error) => {
          console.error('Error:', error);
        });

      if (productResponse) {
        setProducts(productResponse);
      } else {
        console.error('Error: productResponse is undefined');
      }
      let cartItems = userData?.cart
      if (cartItems === undefined) {
        cartItems = placeholderCart;
        cartItems.userId = userData.id;
      }
      setCart(cartItems);
      console.log('cart:', cart);
    } catch (error) {
      console.error('Error:', error);
    }
  };

  // const handleRegister = (email) => {
  //   setValidEmailsState(prevEmails => [...prevEmails, email]);
  // };

const handleRegister = async (email) => {
  let url = `${base_url}/api/Auth/RegisterUser`;
  console.log(url);

  try {
    let response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email: email}),
    });

    if (response.status === 200) {
      console.log(await response.json());
      return true;
    } else {
      return false;
    }
  } catch (error) {
    console.error('Error:', error);
    alert("User backend not reachable");
    return false;
  }
};

  const handleLogout = () => {
    setLoggedIn(false);
  };

  // This function handles adding a product to the cart.
  const handleUpdateCart = (productId, productName, productPrice) => {
    if (!cart) {
      console.error('Cart is undefined');
      return;
    }
    else {
          // Copy the previous cart
          let newCart = { ...cart };
        if (Array.isArray(newCart.products)) {
          // Find the index of the product in the products array
          const productIndex = newCart.products.findIndex(product => product.productId === productId);
          if (productIndex !== -1) {
            // Product exists in the cart, increment its quantity
            console.log('Product exists in the cart, increment its quantity');
            newCart.products[productIndex].productQuantity += 1;
            newCart.products[productIndex].totalPrice = (productPrice * newCart.products[productIndex].productQuantity).toFixed(2);
          } else {
            // Product does not exist in the cart, add it with a quantity of 1
            console.log(`Product does not exist in the cart, add it with a quantity of 1. productId provided: ${productId}`);
            const newProduct = {
              productId,
              productName,
              productPrice,
              productQuantity: 1,
              totalPrice: productPrice 
            };
            newCart.products = [...newCart.products, newProduct];
          }
        }
        setCart(newCart);
    }
};

const handleRemoveFromCart = (productId, cart) => { 
  if (!cart) {
    console.error('Cart is undefined');
    return;
  }
  else {
    // Copy the previous cart
    let newCart = { ...cart };
    if (Array.isArray(newCart.products)) {
      // Find the index of the product in the products array
      const productIndex = newCart.products.findIndex(product => product.productId === productId);
      if (productIndex !== -1) {
        // Product exists in the cart, decrement its quantity
        console.log('Product exists in the cart, decrement its quantity');
        newCart.products[productIndex].productQuantity -= 1;
        newCart.products[productIndex].totalPrice = (newCart.products[productIndex].productPrice * newCart.products[productIndex].productQuantity).toFixed(2);
        if (newCart.products[productIndex].productQuantity === 0) {
          // Remove the product from the cart if its quantity is 0
          newCart.products = newCart.products.filter(product => product.productId !== productId);
        }
      } else {
        console.log('Product does not exist in the cart');
      }
    }
    setCart(newCart);
  }
}


const handleDeleteProduct = async (productId) => {
  let url = `${base_url}/api/Product/DeleteProduct?id=${productId}`;
  let decision = confirm("Are you sure you want to delete this product from the database not just your cart?");
  if (decision) {
    try {
      let response = await fetch(url, { method: 'DELETE' });
      if (response.status === 200) {
        console.log("Product deleted successfully!");
        let newProducts = products.filter(product => product.id !== productId);
        setProducts(newProducts);
        navigate('/');

        // Update the cart state
        if (cart && Array.isArray(cart.products)) {
          let newCart = { ...cart };
          newCart.products = newCart.products.filter(product => product.productId !== productId);
          setCart(newCart);
        }
      } else if (response.status === 404) {
        console.log("Product not found in the database");
      } else {
        throw new Error('Error: response not ok');
      }
    } catch (error) {
      console.error('Error:', error);
    }
  } else {
    console.log("Product not deleted");
  }
};



const purchase = () => {
  if (!cart) {
    console.error('Cart is undefined');
    return;
  }
  else {
    let newCart = { ...cart };
    newCart.orderTotal = newCart.products.reduce((sum, product) => {
      return sum + product.productPrice * product.productQuantity;
    }, 0);
    let url = `${base_url}/api/Orders/CreateOrder`;
    fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(newCart),
    })
      .then(response => {
        if (response.ok) {
          return "Order placed successfully!";
        }
        else {
          throw new Error('Error: response not ok');
        }
      })
      .catch((error) => {
        console.error('Error:', error);
      });
    newCart.products = [];
    newCart.orderTotal = 0;
    setCart(newCart);
  }
};





useEffect(() => {
  let url = `${base_url}/api/Cart/UpdateCart`;

  fetch(url, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(cart),
  })
    .then(response => response.json())
    .then(data => console.log(data))
    .catch((error) => {
      console.error('Error:', error);
    });
}, [cart]); // This will run whenever `cart` changes

// useEffect(() => {
//   let data = [...products];
//   if (brand !== "all") {
//     data = data.filter(data => data.brand === brand);
//   }
//   setProducts(data);
// }, [brand]); // This will run whenever `brand` changes

useEffect(() => {
  if (pull_brand_from_database === 'true') {
    let url = `${base_url}/api/Product/GetProductsByBrand?brand=${brand}`;
    fetch(url)
      .then(response => response.json())
      .then(data => {
        setProducts(data);
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  }
  else {

  let data = products.filter(product => product.brand === brand);
  setBrandData(data);
  }
}, [brand]); // This will run whenever `cart` changes

// useEffect(() => {
//   if (loggedIn) {
//     navigate(`/brand/${brand}`);
//   }
// }, [brandData]);

  return (
      <div className="App">
        <Navbar onLogout={handleLogout} loggedIn={loggedIn} />
        <div className="page-container">
          <Routes>
            <Route path="/login" element={<LoginPage onLogin={handleLogin}
              loggedIn={loggedIn} wrongEmail={wrongEmail} />} />
            <Route path="/register" element={<RegisterPage onRegister={handleRegister}  />} />
            <Route path="/" element={loggedIn ? <ProductList products={products} onAddToCart={handleUpdateCart} onBrandLinkClick={handleBrandLinkClick} onRemoveFromCart={handleRemoveFromCart} cart={cart} /> : <Navigate to="/login" />} />
            <Route path="/product/:id" element={loggedIn ? <ProductDetails products={products} onAddToCart={handleUpdateCart} onBrandLinkClick={handleBrandLinkClick} onRemoveFromCart={handleRemoveFromCart} cart={cart} /> : <Navigate to="/login" />} />
            <Route path="/brand/*" element={loggedIn ?  <ProductList products={brandData} onAddToCart={handleUpdateCart} handleBrandLinkClick={handleBrandLinkClick} /> : <Navigate to="/login" />} />
            <Route path="/cart" element={loggedIn ? <Cart cart={cart} onPurchase={purchase} /> : <Navigate to="/login" />} />
          </Routes>
        </div>
      </div>
  );
}

export default App;