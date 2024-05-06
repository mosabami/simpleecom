// RegisterPage.js
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './LoginPage.css'; // Import the same CSS

const RegisterPage = ({ onRegister }) => {
  const [email, setEmail] = useState('');
  const navigate = useNavigate();

  const handleSubmit = (event) => {
    event.preventDefault();
    onRegister(email);
    navigate('/login'); // Navigate back to the login page
  };

  return (
    <div className="login-page">
      <h1>Register</h1> {/* Add the H1 element */}
      <img src="https://simpleeconsa.blob.core.windows.net/simpleeshop/eshop_homepage.png" alt="Register" className="login-image" />
      <div className="login-form">
        <form onSubmit={handleSubmit}>
          <label>
            Email:
            <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
          </label>
          <div className="button-group">
            <button type="submit">Register</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default RegisterPage;