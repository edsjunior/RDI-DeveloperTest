# RDI-DeveloperTest

Techinical test for software developer at RDI

## API - CreditCards
##### Mock - This mock should be used as FormBody in Postman with Media Type Json

```
{
"cardNumber":4128950786628540,
"cvv":123
}
```

##### Expected Response:
    {
    "Token": "4128950786628540123201912231043",
    "FormatedArray": "4,1,2,0,2,4,0,1,2,3,2,0,1,1,2,2,3,1,0,4,3",
    "RotationArray": "2,3,1"
    }

    

## API - TokenValidations/[Token]
```
Pass [token] generated previously in URI above
```
##### Expected Response: 
     True or False
