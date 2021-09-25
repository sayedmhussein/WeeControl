# Authentication and Authorization
To authenticate and authorize a user, a JWT will be created by the API server, the token should contain claims of the user.

```
{
username*	string -> maxLength: 50, minLength: 3
password*	string -> maxLength: 50, minLength: 3
}
```