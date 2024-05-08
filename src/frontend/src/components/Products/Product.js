// Product.js
import React from 'react';
import { Link } from 'react-router-dom';

const Product = ({ product, handleAddToCart }) => {
  // Use substring to limit the description to the first 100 characters
  const truncatedDescription = product.Description.substring(0, 100);
  let productName = product.name;
  let productPrice = product.price;
  let productId = product.id;

  return (
    <div>
      <img src={`https://simpleecom.blob.core.windows.net/awesomeeshop/${productId}.webp`} alt={productName} style={{ width: '100%', height: 'auto' }} />
      <h2><Link to={`/product/${productId}`}>{productName}</Link></h2>
      <p>{truncatedDescription}...</p>
      <p>${productPrice}</p>
      <button onClick={() => handleAddToCart(productId, productName, productPrice)}>Add to Cart</button>
    </div>
  );
};

export default Product;
