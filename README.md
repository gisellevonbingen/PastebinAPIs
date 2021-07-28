# PastebinAPIs
C# Pastebin APIs

## References

* https://pastebin.com/doc_api

## Dependencies

* [Giselle.Commons](https://github.com/gisellevonbingen/Giselle.Commons)
* [Newtonsoft.Json](https://www.newtonsoft.com/json)
* System.Web


## Examples

### Creating A New Paste

```CSharp
var api = new PastebinAPI();
api.APIKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

var createRequest = new PasteCreateRequest();
createRequest.Code = "Paste Code";
createRequest.Name = "Test Title";
createRequest.ExpireDate = PasteExpireDate.Hour;
createRequest.Private = PastePrivate.Public;
createRequest.Format = "text";
createRequest.UserKey = string.Empty; // Set UserKey In Login,  Empty(or null) In Guest

var url = api.CreatePaste(createRequest);
Console.WriteLine(url);
```

### User Login

```CSharp
var api = new PastebinAPI();
api.APIKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

var loginRequest = new PasteLoginRequest();
loginRequest.Name = "yyyyyyyyyyyyyyy";
loginRequest.Password = "zzzzzzzzzzzzzzzz";

var userKey = api.Login(loginRequest);
Console.WriteLine("UserKey : " + userKey);
```

### List Own Pastes

```CSharp
var api = new PastebinAPI();
api.APIKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

var loginRequest = new PasteLoginRequest();
loginRequest.Name = "yyyyyyyyyyyyyyy";
loginRequest.Password = "zzzzzzzzzzzzzzzz";

var userKey = api.Login(loginRequest);

var listRequest = new PasteListRequest();
listRequest.UserKey = userKey;
listRequest.ResultsLimit = null;

var pastes = api.ListPastes(listRequest);

foreach (var paste in pastes)
{

}
```

### Delete Own Paste

```CSharp
var api = new PastebinAPI();
api.APIKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

var loginRequest = new PasteLoginRequest();
loginRequest.Name = "yyyyyyyyyyyyyyy";
loginRequest.Password = "zzzzzzzzzzzzzzzz";

var userKey = api.Login(loginRequest);

var listRequest = new PasteDeleteRequest();
listRequest.UserKey = userKey;
listRequest.PasteKey = "AAAAAAAA";

var result = api.DeletePaste(listRequest);
Console.WriteLine("Delete Result : " + result);
```

### Catching Exception

```CSharp
try
{
	var api = new PastebinAPI();
	api.APIKey = "invalid api key";

	var loginRequest = new PasteLoginRequest();
	loginRequest.Name = "invalid name";
	loginRequest.Password = "invalid password";

	var userKey = api.Login(loginRequest);

	Console.WriteLine("Should Not Print");
}
catch (PastebinException e)
{
	Console.WriteLine("Error Message : " + e.Message);
}
```
