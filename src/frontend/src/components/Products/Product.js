// Product.js
import React from 'react';
import { Link } from 'react-router-dom';

const Product = ({ product, handleAddToCart }) => {
  if (!product) {
    console.error('Product is undefined');
    return <div>Product is undefined</div>;
  }
  console.log('product:', product);
  // Use substring to limit the description to the first 100 characters
  const truncatedDescription = product.description ? product.description.substring(0, 100) : '';
  let productName = product.name;
  let productPrice = product.price;
  let id = product.id

return (
  truncatedDescription && (
    <div>
      <img src={product.photoURL} alt={productName} style={{ width: '100%', height: 'auto' }} />
      <h2><Link to={`/product/${id}`}>{productName}</Link></h2>
      <p>{truncatedDescription}...</p>
      <p>${productPrice}</p>
      <button onClick={() => handleAddToCart(id, productName, productPrice)}>Add to Cart</button>
    </div>
  )
);
};

export default Product;
