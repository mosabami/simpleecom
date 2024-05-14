// LoginPage.js
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './LoginPage.css'; // Import the CSS

let homepage_pic = process.env.REACT_APP_HOMEPAGE_PIC || 'https://simpleecom.blob.core.windows.net/awesomeeshop/eshop_homepage.png';

const LoginPage = ({ onLogin, loggedIn, wrongEmail }) => {
  const [email, setEmail] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    if (loggedIn) {
      navigate('/');
    }
  }, [loggedIn, navigate]);

  const handleSubmit = (event) => {
    event.preventDefault();
    onLogin(email);
  };

  const handleRegister = () => {
    navigate('/register');
  };

  return (
    <div className="login-page">
       <h1>Login</h1> {/* Add the H1 element */}
      <img src={homepage_pic} alt="Login" className="login-image" />
      <div className="login-form">
        <form onSubmit={handleSubmit}>
          <label>
            Email:
            <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
          </label>
          <div className="button-group">
            <button type="submit">Login</button>
            <button type="button" onClick={handleRegister}>Registration Page</button>
          </div>
        </form>
        <div>
        {wrongEmail && <p style={{ color: 'red' }}>Please enter a registered email address or click the Register button</p>}
        </div>
      </div>
    </div>
  );
};

export default LoginPage;