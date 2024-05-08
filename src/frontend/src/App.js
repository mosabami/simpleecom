// App.js
import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import './App.css';
import ProductList from './components/Products/ProductList';
import Navbar from './components/Navbar/Navbar';
import ProductDetails from './components/Products/ProductDetails';
import Cart from "./components/Cart/Cart";
import LoginPage from './components/LoginRegistration/LoginPage';
import RegisterPage from './components/LoginRegistration/RegisterPage';
// import productsData from './catalog.json';
import placeholderCart from './placeholderCart.json';
let base_url = process.env.REACT_APP_API_BASE_URL || '';
console.log(`base_url: ${base_url}`);


function App() {
  const [loggedIn, setLoggedIn] = useState(false);
  const [wrongEmail, setwrongEmail] = useState(false);
  const [cart, setCart] = useState({});
  const [userID, setUserID] = useState("");
  const [products, setProducts] = useState([]);

  const handleLogin = async (email) => {
    let login_endpoint = `${base_url}/api/Auth/Login?email=${encodeURIComponent(email)}`;
    console.log(`login_endpoint: ${login_endpoint}`);

    try {
      let response = await fetch(login_endpoint);
      if (!response.ok) {
        setwrongEmail(true);
        throw new Error('Email not registered');
      }
      let data = await response.json();
      setLoggedIn(true);
      setUserID(data["id"]);
      setwrongEmail(false);
      let productResponse = await fetch(`${base_url}/api/Product/GetProducts`)
        .then(response => response.json())
        .catch((error) => {
          console.error('Error:', error);
        });

      if (productResponse) {
        setProducts(productResponse);
        console.log('productResponse:', productResponse);
        console.log('products:', products);
      } else {
        console.error('Error: productResponse is undefined');
      }
      setCart(productResponse?.cart ?? placeholderCart);
    } catch (error) {
      console.error('Error:', error);
    }
  };

  // const handleRegister = (email) => {
  //   setValidEmailsState(prevEmails => [...prevEmails, email]);
  // };

  const handleRegister = (email) => {
    let url = `${base_url}/api/Auth/RegisterUser`;
    console.log(url);

    return fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email: email, id: "ee", firstName: "hi", lastName: "aa", type: "user" }),
    })
      .then(response => response.json())
      .then(data => {
        console.log(data);
        return Math.random(); // return a random number
      })
      .catch((error) => {
        console.error('Error:', error);
      });

  };

  const handleLogout = () => {
    setLoggedIn(false);
  };


  const clearCart = () => {
    setCart({});
    let url = `${base_url}/api/Cart/DeleteCart`;

    fetch(url, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ userId: userID }), // replace userID with the actual userID
    })
      .then(response => response.json())
      .then(data => console.log(data))
      .catch((error) => {
        console.error('Error:', error);
      });
  };

  // This function handles adding a product to the cart.
  const handleAddToCart = (id, productName, productPrice) => {
    setCart(prevCart => {
      // Find the index of the product in the products array
      const productIndex = prevCart.products.findIndex(product => product.id === id);

      if (productIndex !== -1) {
        // Product exists in the cart, increment its quantity
        const newCart = { ...prevCart };
        newCart.products[productIndex].productQuantity += 1;
        return newCart;
      } else {
        // Product does not exist in the cart, add it with a quantity of 1
        const newProduct = {
          id,
          productName,
          productPrice,
          productQuantity: 1
        };
        return { ...prevCart, products: [...prevCart.products, newProduct] };
      }
    });

    let url = `${base_url}/api/Cart/UpdateCart`;

    fetch(url, {
      method: 'PUT',
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

  };

  return (
    <Router>
      <div className="App">
        <Navbar onLogout={handleLogout} loggedIn={loggedIn} />
        <div className="page-container">
          <Routes>
            <Route path="/login" element={<LoginPage onLogin={handleLogin}
              loggedIn={loggedIn} wrongEmail={wrongEmail} />} />
            <Route path="/register" element={<RegisterPage onRegister={handleRegister} />} />
            <Route path="/" element={loggedIn ? <ProductList products={products} handleAddToCart={handleAddToCart} /> : <Navigate to="/login" />} />
            <Route path="/product/:id" element={loggedIn ? <ProductDetails products={products} onAddToCart={handleAddToCart} cart={cart} /> : <Navigate to="/login" />} />
            <Route path="/cart" element={loggedIn ? <Cart cart={cart} clearCart={clearCart} /> : <Navigate to="/login" />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;