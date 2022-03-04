# HttpEdit
Proxy, redord, edit and resend RAW HTTP requests

### Usage:
``` bash
# Listen to incoming requests and print to screen
httpedit record 0.0.0.0:80

# Listen to requests and save to file
httpedit record 0.0.0.0:80 > request.txt

# Send request from file
httpedit send target.com:80 request.txt
```