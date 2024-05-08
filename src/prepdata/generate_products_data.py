import json
import requests
import os

# Load environment variables from .env file

class GenerateProductsData():
    def __init__(self, storageBaseUrl):
        # Fetch the data from the URL
        try:
            response = requests.get('https://raw.githubusercontent.com/dotnet/eShop/main/src/Catalog.API/Setup/catalog.json')
            response.raise_for_status()  # Raises a HTTPError if the response status is 4xx, 5xx
            self.catalog_data = response.json()
            print('Data fetched successfully from the Dotnet\'s repo.')
        except (requests.exceptions.RequestException, ValueError):
            print('Failed to fetch data from the Dotnet\'s repo. Using local data instead.')
            with open('catalog.json') as f:
                self.catalog_data = json.load(f)
        # Get the storage base URL from environment variables
        self.storageBaseUrl = storageBaseUrl
        print(f"Storage Base URL: {self.storageBaseUrl}")
        self.generate_products_data()

    def convert_to_model(self):
        "function to convert the catalog data to the format expected by the API"
        catalog_data = []
        for item in self.catalog_data:
            new_item = {}
            prodId = str(item.get('Id'))
            print(f'Creating product with ID {type(prodId)}')
            new_item['id'] = prodId
            new_item['name'] = item.get('Name')
            new_item['description'] = item.get('Description')
            new_item['photoURL'] = f'{self.storageBaseUrl}/{prodId}.webp'
            new_item['inventory'] = 100
            new_item['price'] = item.get('Price')
            new_item['brand'] = item.get('Brand')
            new_item['type'] = item.get('Type')
            catalog_data.append(new_item)
        return catalog_data

    # use the local catalog data if the data returned by the API is not in the format expected
    def generate_products_data(self):
        try:
            self.product_data = self.convert_to_model()
            print('Data converted successfully using API data.')
        except KeyError:
            with open('catalog.json') as f:
                self.catalog_data = json.load(f)
            self.product_data = self.convert_to_model()
            print('Data converted successfully using local data.')

        # Write the updated list back to the file
        with open('productsData.json', 'w') as file:
            json.dump(self.product_data, file, indent=2)
        print('Conversion completed successfully.')

# Run the script to generate the products data if the script is run directly
if __name__ == '__main__':
    from dotenv import load_dotenv
    load_dotenv()
    storageBaseUrl =  os.getenv('STORAGE_BASE_URL', 'https://simpleecom.blob.core.windows.net/awesomeeshop')
    print(f'storageBaseUrl: {storageBaseUrl}')
    GenerateProductsData(storageBaseUrl)
