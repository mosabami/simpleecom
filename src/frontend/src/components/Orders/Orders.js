// Cart.js
import React from 'react';
import './Orders.css'; // Import the CSS

const Orders = ({ orders }) => {
    let total = orders.reduce((sum, order) => {
        return sum + order.orderTotal;
    }, 0).toFixed(2);
    return (
        <div>
            <h2>Orders</h2>
            <table className="orders-table"> {/* Add the CSS class */}
                <thead>
                    <tr>
                        <th>Order ID</th>
                        <th>Number of products</th>
                        <th>Order Total</th>
                    </tr>
                </thead>
                <tbody>
                    {orders.map(order => {
                        return order ? (
                            <tr key={order.id}>
                                <td>{order.id}</td>
                                <td>{order.products.length}</td>
                                <td>${order.orderTotal}</td>
                            </tr>
                        ) : null;
                    })}
                </tbody>
                <tfoot>
                    <tr>
                        <td colSpan="2">Total</td>
                        <td>${total}</td>
                    </tr>
                </tfoot>
            </table>
        </div>
    );
};

export default Orders;