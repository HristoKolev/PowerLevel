#!/usr/bin/env bash

TMP_SOURCE="${BASH_SOURCE[0]}"
while [ -h "$TMP_SOURCE" ]; do
  SCRIPT_PATH="$( cd -P "$( dirname "$TMP_SOURCE" )" >/dev/null 2>&1 && pwd )"
  TMP_SOURCE="$(readlink "$TMP_SOURCE")"
  [[ $TMP_SOURCE != /* ]] && TMP_SOURCE="$SCRIPT_PATH/$TMP_SOURCE"
done
SCRIPT_PATH="$( cd -P "$( dirname "$TMP_SOURCE" )" >/dev/null 2>&1 && pwd )"

OUTPUT_PATH=$1

if [[ -z "$OUTPUT_PATH" ]]; then
  echo "Please provide an output path as first parameter."
  exit 1;
fi

set -eu -o pipefail

VAR_PRIVATE_KEY=$(mktemp)
VAR_PUBLIC_KEY=$(mktemp)
VAR_CERTIFICATE_CRT=$(mktemp)

openssl genpkey -algorithm RSA -out "$VAR_PRIVATE_KEY" -pkeyopt rsa_keygen_bits:4096
openssl rsa -in "$VAR_PRIVATE_KEY" -pubout > "$VAR_PUBLIC_KEY"
openssl req -key "$VAR_PRIVATE_KEY" -new -nodes -x509 -out "$VAR_CERTIFICATE_CRT" -subj "/C=BG/ST=Sofia/L=Sofia/O=xdxd/CN=admin@xdxd.eu"
openssl pkcs12 -export -out "$OUTPUT_PATH" -inkey "$VAR_PRIVATE_KEY" -in "$VAR_CERTIFICATE_CRT" -passout pass:

rm -f "$VAR_PRIVATE_KEY" "$VAR_PUBLIC_KEY" "$VAR_CERTIFICATE_CRT"
