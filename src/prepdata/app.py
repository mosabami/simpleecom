from flask import Flask, request, jsonify
import requests
import json
import os
from generate_products_data import GenerateProductsData  # Import the GenerateProductsData class


storageBaseUrl =  os.getenv('STORAGE_BASE_URL', 'https://simpleecom.blob.core.windows.net/awesomeeshop')

app = Flask(__name__)


@app.route('/prepdata/send_data', methods=['GET'])
def send_data():
    """Check to see if the product data exists in memory. If it doesn't, 
    generate the data using the GenerateProductsData class, and then send the data to the API."""
    if not os.path.exists('productsData.json'):
        GenerateProductsData(storageBaseUrl).generate_products_data()
    with open('productsData.json') as f:
        data = json.load(f)

    for item in data:
        response = requests.post('/api/Product/CreateProduct', json=item)
        if response.status_code == 409:
            response = requests.put('/api/Product/UpdateProduct', json=item)
        print(f'Response status code: {response.status_code}')
    return 'Data uploaded', 200

@app.route('/prepdata/delete_data', methods=['POST'])
def delete_data():
    ids = request.json
    for id in ids:
        response = requests.delete(f'/api/Product/DeleteProduct/{id}')
        print(f'Response status code for ID {id}: {response.status_code}')
    return 'Data deleted', 200

if __name__ == '__main__':
    app.run(debug=True)