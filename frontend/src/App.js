// App.js
import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import './App.css';
import ProductList from './ProductList';
import Navbar from './Navbar';
import ProductDetails from './ProductDetails';
import Cart from './Cart';
import LoginPage from './LoginPage';
import RegisterPage from './RegisterPage';
import productsData from './catalog.json';

function App() {
  const products = productsData;
  const [loggedIn, setLoggedIn] = useState(false);
  const [wrongEmail, setwrongEmail] = useState(false);
  const [order, setOrder] = useState({});
  const validEmails = ['user1@example.com', 'user2@example.com']; // Add more valid emails as needed
  const [validEmailsState, setValidEmailsState] = useState(validEmails);


  const handleLogin = (email) => {
    if (validEmailsState.includes(email)) {
      setLoggedIn(true);
      setwrongEmail(false);
    }
    else {
      setwrongEmail(true);
    }
  };

  const handleRegister = (email) => {
    setValidEmailsState(prevEmails => [...prevEmails, email]);
  };

  const handleLogout = () => {
    setLoggedIn(false);
  };

  const clearOrder = () => {
    setOrder({});
  };

  const handleAddToCart = (productId) => {
    setOrder(prevOrder => ({
      ...prevOrder,
      [productId]: (prevOrder[productId] || 0) + 1,
    }));
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
          <Route path="/cart" element={loggedIn ? <Cart order={order} products={products} clearOrder={clearOrder} /> : <Navigate to="/login" />} />
        </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;