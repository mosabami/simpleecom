import json

# Read the JSON file
with open('catalog.json', 'r') as file:
    catalog_data = json.load(file)

# Modify 'Id' to 'id' for all dictionaries in the list
for item in catalog_data:
    if 'Id' in item:
        item['productId'] = item.pop('Id')
        item.pop('Embedding')
        item['Inventory'] = 100

# Write the updated list back to the file
with open('catalog.json', 'w') as file:
    json.dump(catalog_data, file, indent=2)

print('Conversion completed successfully.')
