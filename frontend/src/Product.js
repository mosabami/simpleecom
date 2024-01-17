// Product.js
import React from 'react';
import { Link } from 'react-router-dom';

const Product = ({ product, handleAddToCart }) => (
  <div>
    <img src={product.image} alt={product.name} style={{ width: '100%', height: 'auto' }} />
    <h2><Link to={`/product/${product.id}`}>{product.name}</Link></h2>
    <p>{product.description}</p>
    <p>${product.price}</p>
    <button onClick={() => handleAddToCart(product.id)}>Add to Cart</button>
  </div>
);

export default Product;