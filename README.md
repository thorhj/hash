# hash
A command-line tool to calculate hashes and checksums.

- example usage: > hash sha1 "my text"
- verbs: different hashing algorithms
  - md5
  - sha1
  - sha256
  -etc.
- options:
  - --secret (-s): enter the text on a new line without showing any keys
  - --checksum (-c) <path>: checksum of a file, alternatively a list of checksums in the given directory or the current directory if no path is given.
  
