# ReceiptProcessor

A containerized application for processing receipts.

## Quick Start

1. Pull the Docker image:

docker pull tl11717/receiptprocessor:latest

2. Run the container:

docker run -d -p 8080:80 tl11717/receiptprocessor:latest

3. The API is now accessible at `http://localhost:8080`

## API Endpoints

### 1. Process Receipt
- **POST** `/receipts/process`
- Body: JSON (see example below)

### 2. Get Points
- **GET** `/receipts/{id}/points`

## Example Usage with Postman

1. Process Receipt:
- POST to `http://localhost:8080/receipts/process`
- Body (raw JSON):
  ```json
  {
    "retailer": "Target",
    "purchaseDate": "2022-01-01",
    "purchaseTime": "13:01",
    "items": [
      {
        "shortDescription": "Mountain Dew 12PK",
        "price": "6.49"
      },{
        "shortDescription": "Emils Cheese Pizza",
        "price": "12.25"
      },{
        "shortDescription": "Knorr Creamy Chicken",
        "price": "1.26"
      },{
        "shortDescription": "Doritos Nacho Cheese",
        "price": "3.35"
      },{
        "shortDescription": "   Klarbrunn 12-PK 12 FL OZ  ",
        "price": "12.00"
      }
    ],
    "total": "35.35"
  }
  ```
- Save the returned ID

2. Get Points:
- GET `http://localhost:8080/receipts/{id}/points`
- Replace `{id}` with the ID from step 1
- **Point total should be 28**


## Managing the Container

- List containers: `docker ps`
- Stop container: `docker stop <container_id>`
- Remove container: `docker rm <container_id>`

For more detailed instructions or troubleshooting, please refer to the full documentation.
