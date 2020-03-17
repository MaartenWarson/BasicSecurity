# BasicSecurity
 
## General Summary
The goal of this project is to create an application to let registered users communicate with eachother in a secured way. A user can register itself where its password will be hashed (AES) in combination with a created salt.

A user is able to select a file and send this file encrypted (AES) to another registered user. The user that receives the encrypted file - and only this user(!) - can decrypt and open the received file.

Stenography has also been applied in this project. A registered user is able to hide a secret message in an image and send this image to another registered user. This user - and only this user(!) - can read the secret message hidden in this picture.
