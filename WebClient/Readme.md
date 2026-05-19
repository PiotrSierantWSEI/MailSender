
# Generowanie biblioteki ts

## Użyte narzędzie

`https://github.com/ferdikoomen/openapi-typescript-codegen`

### Dokumentacja

`https://github.com/ferdikoomen/openapi-typescript-codegen/wiki/Basic-usage`

Komenda do generowania biblioteki

```bash
npx openapi-typescript-codegen --input ./openapi.yaml --output ./client-ts --client fetch
```

### Transpilacja do javascript

```bash
npx tsc --init
npx tsc
```
