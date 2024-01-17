
// ProductList.js
import React from 'react';
import Product from './Product';

const ProductList = ({ products, handleAddToCart }) => (
  <div className="product-list" style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '1rem' }}>
    {products.map(product => (
      <Product key={product.id} product={product} handleAddToCart={handleAddToCart}  />
    ))}
  </div>
);

export default ProductList;