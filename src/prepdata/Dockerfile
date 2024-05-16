# Use an official Python runtime as a parent image
FROM python:3.12-slim

#Set the working directory in the Docker container
WORKDIR /code
#Copy the dependencies file to the working directory
COPY requirements.txt .

#Install the dependencies
RUN pip install -r requirements.txt

#Copy the Flask app code to the working directory
COPY . .
#Run the container
EXPOSE 5000
CMD [ "python", "./app.py" ]