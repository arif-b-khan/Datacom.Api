# Datacom.Api

### Pull docker image

```
docker pull arifbkhan/datacom-api:latest
```

### Run docker image

```
docker run --rm -d datacom-api:latest
```

### Run following curl command

```
curl -X 'POST'   'http://localhost:8081/api/Payslip'   -H 'accept: text/plain'   -H 'Content-Type: application/json'   -d '[
  {
    "firstName": "Arif",
    "lastName": "Khan",
    "annualSalary": 60050,
    "superRate": 9,
    "payPeriod": 3
  }
]'
```

### Command to build the project using docker

```
docker build --no-cache --pull --rm -f "Dockerfile" -t datacom-api:latest . --progress=plain
```
