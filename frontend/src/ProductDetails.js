// ProductDetails.js
import React from 'react';
import { useParams } from 'react-router-dom';
import './ProductDetails.css';

const ProductDetails = ({ products, onAddToCart, order }) => {
  const { id } = useParams();
  const product = products.find(product => product.id === id);

  if (!product) {
    return <div>Loading...</div>;
  }

  return (
    <div className="product-details">
      <div className="product-image">
        <img src={product.image} alt={product.name} />
      </div>
      <div className="product-info">
        <h2>{product.name}</h2>
        <p>{product.description}</p>
        <p><strong>Price:</strong>${product.price}</p>
        <p><strong>Cart quantity:</strong> {order[product.id] || 0}</p>
        <button onClick={() => onAddToCart(product.id)}>Add to Cart</button>
      </div>
    </div>
  );
};


export default ProductDetails;
