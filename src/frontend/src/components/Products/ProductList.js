import React from 'react';
import Product from './Product';
import './ProductList.css';

const generateProduct = (product, handleAddToCart) => {
  if (product.description && product.description.trim().length >= 10) {
    return <Product key={product.id} product={product} handleAddToCart={handleAddToCart}  />
  }
  else {
    return null;
  }
}

const ProductList = ({ products, handleAddToCart }) => (
  // console.log(`products: ${products}`)
  <div className="product-list">
    {products.map(product => (
      generateProduct(product, handleAddToCart)    
    ))}
  </div>
);

export default ProductList;