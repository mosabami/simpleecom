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

let base_url = process.env.BASE_URL || 'http://localhost:';
let auth_url = process.env.AUTH_URL || '';
console.log(`auth_url: ${auth_url}`);
let products_url = process.env.PRODUCTS_URL || base_url +  '8084';
let cart_url = process.env.CART_URL || "";
let orders_url = process.env.ORDERS_URL || base_url +  '8082';

function App() {
  const [loggedIn, setLoggedIn] = useState(false);
  const [wrongEmail, setwrongEmail] = useState(false);
  const [order, setOrder] = useState({});
  const [userID, setUserID] = useState(""); 


  // const handleLogin = (email) => {
  //   if (validEmailsState.includes(email)) {
  //     setLoggedIn(true);
  //     setwrongEmail(false);
  //   }
  //   else {
  //     setwrongEmail(true);
  //   }
  // };

const handleLogin = async (email) => {
    let login_endpoint = `${auth_url}/api/Auth/Login?email=${encodeURIComponent(email)}`;
    let cart_endpoint = `${cart_url}/api/Cart/GetCart?userId=`;

    console.log(`login_endpoint: ${login_endpoint}`);

    try {
        let response = await fetch(login_endpoint);
        if (!response.ok) {
            setwrongEmail(true);
            throw new Error('Email not registered');
        }

        let data = await response.json();
        let cart = {};
        setLoggedIn(true);
        setUserID(data["id"]);
        setwrongEmail(false);
        products = await fetch(products_url).then(response => response.json());
        response = await fetch(cart_endpoint + userID);
        if (!response.ok) {
            cart = placeholderCart;
            placeholderCart.userId = userID;
        }
        else {
          cart = await response.json();
        }
        setOrder(cart);
    } catch (error) {
        console.error('Error:', error);
    }
};

  // const handleRegister = (email) => {
  //   setValidEmailsState(prevEmails => [...prevEmails, email]);
  // };

  const handleRegister = (email) => {
    let url = `${auth_url}/api/Auth/RegisterUser`;
    console.log(url);
    
    return fetch(url, {
      method: 'POST',
      headers: {
          'Content-Type': 'application/json',
      },
      body: JSON.stringify({ email: email,id:"ee",firstName:"hi",lastName:"aa",type:"user" }),
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

  // const clearOrder = () => {
  //   setOrder({});
  // };

  const clearOrder = () => {
    setOrder({});
    let url = `${cart_url}/api/Cart/DeleteCart`;

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
const handleAddToCart = (productId, productName, productPrice) => {
  setOrder(prevOrder => {
    // Find the index of the product in the products array
    const productIndex = prevOrder.products.findIndex(product => product.productId === productId);

    if (productIndex !== -1) {
      // Product exists in the order, increment its quantity
      const newOrder = { ...prevOrder };
      newOrder.products[productIndex].productQuantity += 1;
      return newOrder;
    } else {
      // Product does not exist in the order, add it with a quantity of 1
      const newProduct = {
        productId,
        productName,
        productPrice,
        productQuantity: 1
      };
      return { ...prevOrder, products: [...prevOrder.products, newProduct] };
    }
  });

    let url = `${cart_url}/api/Cart/UpdateCart`;

    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(order),
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
          loggedIn={loggedIn} wrongEmail = {wrongEmail} />} />
          <Route path="/register" element={<RegisterPage onRegister={handleRegister} />} />
          <Route path="/" element={loggedIn ? <ProductList products={products} handleAddToCart={handleAddToCart} /> : <Navigate to="/login" />} />
          <Route path="/product/:id" element={loggedIn ? <ProductDetails products={products} onAddToCart={handleAddToCart} order={order} /> : <Navigate to="/login" />} />
          <Route path="/cart" element={loggedIn ? <Cart order={order}  clearOrder={clearOrder} /> : <Navigate to="/login" />} />
        </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;