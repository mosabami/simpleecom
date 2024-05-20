// Navbar.js
import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css'; // Import the CSS

const Navbar = ({ onLogout, loggedIn, server }) => (
  <nav className="navbar">
    <div className="navbar-left">
      <Link to="/" className="navbar-button navbar-item">Home</Link>
    </div>
    <div className="navbar-left">
      <p className="navbar-item">{server}</p>
    </div>
    <div className="navbar-right">
      <Link to="/orders" className="navbar-button navbar-item">Orders</Link>
      <Link to="/cart" className="navbar-button navbar-item">Cart</Link>
      {loggedIn && <button onClick={onLogout}>Sign Out</button>}
    </div>
  </nav>
);

export default Navbar;