import React from 'react';
import Product from './Product';
import './ProductList.css';


const generateProduct = (product, onAddToCart, onBrandLinkClick, onRemoveFromCart, cart) => {
  // only show products with descriptions longer than 10 characters
  if (product.description && product.description.trim().length >= 10) {
    return <Product key={product.id} product={product} onAddToCart={onAddToCart} onBrandLinkClick={onBrandLinkClick} 
      onRemoveFromCart={onRemoveFromCart} cart={cart}  />
  }
  else {
    return null;
  }
}

const ProductList = ({ products, onAddToCart, onBrandLinkClick, onRemoveFromCart, cart }) => (

  // console.log(`products: ${products}`)
  <div className="product-list">
    {products.map(product => (
      generateProduct(product, onAddToCart, onBrandLinkClick, onRemoveFromCart, cart)    
    ))}
  </div>
);

export default ProductList;