// ProductDetails.js
import React from 'react';
import { useParams } from 'react-router-dom';
import './ProductDetails.css';

const ProductDetails = ({ products, onAddToCart, cart }) => {
  const { id } = useParams();
  const product = products.find(product => String(product.productId) === id);
  console.log('product in details:', product);
  let productName = product.name;
  let productPrice = product.price;
  let productId = product.id; // This is the product ID from the database, needs to be productId because id is the id from the URL

  if (!product) {
    return <div>Loading...</div>;
  }

  return (
    <div className="product-details">
      <div className="product-image">
        <img src={product.photoURL} alt={productName} />
      </div>
      <div className="product-info">
        <h2>{product.name}</h2>
        <p>{product.description}</p>
        <p><strong>Price:</strong>${product.price}</p>
        <p><strong>Cart quantity:</strong> {cart[productId] || 0}</p>
        <button onClick={() => onAddToCart(productId, productName, productPrice)}>Add to Cart</button>
      </div>
    </div>
  );
};


export default ProductDetails;
