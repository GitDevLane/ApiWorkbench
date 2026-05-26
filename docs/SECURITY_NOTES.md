# Security Notes

ApiWorkbench is intended to test connections safely and transparently.

Because connection testing may involve API keys, database credentials, cloud credentials, and private endpoints, security must be considered from the beginning.

## Do Not Commit Secrets

Never commit:

- API keys
- Database passwords
- AWS access keys
- Bearer tokens
- Private connection strings
- Real customer data
- Local `.env` files
- Local development appsettings files with secrets

## Git-Ignored Secret Files

The repo should ignore local files such as:

```text
.env
appsettings.Development.json
appsettings.Local.json
secrets.json
*.db
*.sqlite
*.sqlite3

