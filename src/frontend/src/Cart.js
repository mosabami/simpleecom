// Cart.js
import React from 'react';
import './Cart.css'; // Import the CSS

const Cart = ({ order, products, clearOrder }) => {
    const total = Object.keys(order).reduce((sum, productId) => {
        const product = products.find(product => product.id === productId);
        return product ? sum + product.Price * order[productId] : sum;
    }, 0);

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
                    </tr>
                </thead>
                <tbody>
                    {Object.keys(order).map(productId => {
                        const product = products.find(product => product.id === productId);
                        return product ? (
                            <tr key={productId}>
                                <td>{product.Name}</td>
                                <td>${product.Price}</td>
                                <td>{order[productId]}</td>
                                <td>${product.Price * order[productId]}</td>
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
            <button onClick={clearOrder} className="clear-cart-button">Place Order</button> {/* Add the CSS class */}
        </div>
    );
};

export default Cart;