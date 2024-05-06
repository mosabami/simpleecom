// Product.js
import React from 'react';
import { Link } from 'react-router-dom';

const Product = ({ product, handleAddToCart }) => {
  // Use substring to limit the description to the first 100 characters
  const truncatedDescription = product.Description.substring(0, 100);

  return (
    <div>
      <img src={`https://simpleecom.blob.core.windows.net/awesomeeshop/${product.id}.webp`} alt={product.Name} style={{ width: '100%', height: 'auto' }} />
      <h2><Link to={`/product/${product.id}`}>{product.Name}</Link></h2>
      <p>{truncatedDescription}...</p>
      <p>${product.Price}</p>
      <button onClick={() => handleAddToCart(product.id)}>Add to Cart</button>
    </div>
  );
};

export default Product;
