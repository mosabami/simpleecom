// RegisterPage.js
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './LoginPage.css'; // Import the same CSS

let homepage_pic = process.env.REACT_APP_HOMEPAGE_PIC || 'https://simpleecom.blob.core.windows.net/awesomeeshop/eshop_homepage.png';

const RegisterPage = ({ onRegister }) => {
  const [email, setEmail] = useState('');
  const [regFail, setregFail] = useState(false); // Add this line
  const navigate = useNavigate();

const handleSubmit = async (event) => {
  event.preventDefault();
  let reg = await onRegister(email);
  console.log("reg", reg);
  if (reg) {
    navigate('/login');
  } else {
    setregFail(true);
  }
};

  return (
    <div className="login-page">
      <h1>Register</h1> {/* Add the H1 element */}
      <img src={homepage_pic} alt="Register" className="login-image" />
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
        <div>
        {regFail && <p style={{ color: 'red' }}>Email already registered or registration failed</p>}
        </div>
      </div>
    </div>
  );
};

export default RegisterPage;