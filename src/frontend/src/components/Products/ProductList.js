import React from 'react';
import Product from './Product';
import './ProductList.css';


const generateProduct = (product, onAddToCart, handleBrandLinkClick) => {
  if (product.description && product.description.trim().length >= 10) {
    return <Product key={product.id} product={product} onAddToCart={onAddToCart} handleBrandLinkClick={handleBrandLinkClick}  />
  }
  else {
    return null;
  }
}

const ProductList = ({ products, onAddToCart, handleBrandLinkClick }) => (

  // console.log(`products: ${products}`)
  <div className="product-list">
    {products.map(product => (
      generateProduct(product, onAddToCart,  handleBrandLinkClick)    
    ))}
  </div>
);

export default ProductList;