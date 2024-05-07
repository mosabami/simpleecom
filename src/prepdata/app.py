from flask import Flask, request, jsonify
import requests
import json
import os
from generate_products_data import GenerateProductsData  # Import the GenerateProductsData class

from dotenv import load_dotenv
load_dotenv()
# storageBaseUrl =  os.getenv('STORAGE_BASE_URL', 'https://simpleecom.blob.core.windows.net/awesomeeshop')
# apiBaseUrl = os.getenv('API_BASE_URL',"")

try:
    storageBaseUrl =  os.environ['STORAGE_BASE_URL']
except:
    storageBaseUrl = 'https://simpleecom.blob.core.windows.net/awesomeeshop'
try:
    apiBaseUrl =  os.environ['API_BASE_URL']
except:
    apiBaseUrl = ''

print(f'storageBaseUrl: {storageBaseUrl}')
print("apiBaseUrl: ", apiBaseUrl)

app = Flask(__name__)

@app.route('/prepdata')
def home():
    return 'Welcome to the prepdata API'

@app.route('/prepdata/send_data', methods=['POST'])
def send_data():
    """Check to see if the product data exists in memory. If it doesn't, 
    generate the data using the GenerateProductsData class, and then send the data to the API."""
    print(f'storageBaseUrl: {storageBaseUrl}')
    print("apiBaseUrl: ", apiBaseUrl)
    if not os.path.exists('productsData.json'):
        GenerateProductsData(storageBaseUrl).generate_products_data()
    with open('productsData.json') as f:
        data = json.load(f)

    for item in data:
        print(f'Creating product with ID {item["id"]}')
        response = requests.post(f'{apiBaseUrl}/api/Product/CreateProduct', json=item)
        if response.status_code == 409:
            print(f'Product with ID {item["id"]} already exists. Updating product instead.')
            response = requests.put(f'{apiBaseUrl}/api/Product/UpdateProduct', json=item)
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
    app.run(host='0.0.0.0', port=5000, debug=True)