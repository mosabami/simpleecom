// Product.js
import React from 'react';
import { Link } from 'react-router-dom';

const Product = ({ product, onAddToCart, onBrandLinkClick, onRemoveFromCart, cart }) => {
  if (!product) {
    console.error('Product is undefined');
    return <div>Product is undefined</div>;
  }

  // Use substring to limit the description to the first 100 characters
  const truncatedDescription = product.description ? product.description.substring(0, 100) : '';
  let productName = product.name;
  let productPrice = product.price;
  let productId = product.id
  let brand = product.brand;



return (
  truncatedDescription && (
    <div>
      <img src={product.photoURL} alt={productName} style={{ width: '100%', height: 'auto' }} />
      <h2><Link to={`/product/${productId}`}>{productName}</Link></h2>
      <h3><strong>Brand: </strong><Link to={`/brand/${encodeURIComponent(brand)}`} onClick={() => onBrandLinkClick(brand)}>{brand}</Link></h3>
      <p>{truncatedDescription}...</p>
      <p>${productPrice}</p>
      <button style={{marginBottom: '10px'}} onClick={() => onAddToCart(productId, productName, productPrice)}>Add to Cart</button>
      <button style={{backgroundColor: '#E46A4C'}} onClick={() => onRemoveFromCart(productId, cart)}>Remove from cart</button>
    </div>
  )
);
};

export default React.memo(Product);
