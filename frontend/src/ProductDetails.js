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
        <img src={`https://simpleeconsa.blob.core.windows.net/simpleeshop/${product.id}.webp`} alt={product.Name} />
      </div>
      <div className="product-info">
        <h2>{product.Name}</h2>
        <p>{product.Description}</p>
        <p><strong>Price:</strong>${product.Price}</p>
        <p><strong>Cart quantity:</strong> {order[product.id] || 0}</p>
        <button onClick={() => onAddToCart(product.id)}>Add to Cart</button>
      </div>
    </div>
  );
};


export default ProductDetails;
