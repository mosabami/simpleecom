

// Navbar.js
import React from 'react';
import { Link } from 'react-router-dom';

const Navbar = ({ onLogout, loggedIn }) => (
  <nav className="navbar">
    <Link to="/" className="navbar-button">Home</Link>
    <Link to="/cart" className="navbar-button">Cart</Link>
    {loggedIn && <button onClick={onLogout}>Sign Out</button>}
  </nav>
);

export default Navbar;