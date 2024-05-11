// Cart.js
import React from 'react';
import './Cart.css'; // Import the CSS

const Cart = ({ cart,  onPurchase, onRemoveFromCart }) => {
    let total = cart.products.reduce((sum, product) => {
        return sum + product.productPrice * product.productQuantity;
    }, 0).toFixed(2);

    return (
        <div>
            <h2>Cart</h2>
            <table className="cart-table"> {/* Add the CSS class */}
                <thead>
                    <tr>
                        <th>Product Name</th>
                        <th>Unit Price</th>
                        <th>Quantity</th>
                        <th>Total Price</th>
                        <th>Reduce</th>
                    </tr>
                </thead>
                <tbody>
                    {cart.products.map(product => {
                        return product ? (
                            <tr key={product.productId}>
                                <td>{product.productName}</td>
                                <td>${product.productPrice}</td>
                                <td>{product.productQuantity}</td>
                                <td>${(product.productPrice * product.productQuantity).toFixed(2)}</td>
                                <td><button style={{backgroundColor: '#E46A4C'}} onClick={() => onRemoveFromCart(product.productId, cart)}>Remove from cart</button></td>
                            </tr>
                        ) : null;
                    })}
                </tbody>
                <tfoot>
                    <tr>
                        <td colSpan="3">Total</td>
                        <td>${total}</td>
                    </tr>
                </tfoot>
            </table>
            <button onClick={onPurchase} className="clear-cart-button">Place Order</button> {/* Add the CSS class */}
        </div>
    );
};

export default Cart;