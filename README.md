# AmazonAppBackend
<details>
<summary><h2>Profile API Documentation</h2></summary>

## GetProfile

Retrieves the profile information for a given username.

**Endpoint:** `/api/profile/{username}`

**Method:** GET

### Request

- `{username}` (path parameter): The username of the profile to retrieve.

### Response

- Status: 200 OK
- Body:
  - `Username` (string): The username of the profile.
  - `Email` (string): The email address of the profile.
  - `Password` (string): The password of the profile.
  - `FirstName` (string): The first name of the profile.
  - `LastName` (string): The last name of the profile.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the provided username format is invalid.
- Status: 404 Not Found
  - Body: Indicates that no profile with the specified username was found.

### Description

This endpoint allows you to retrieve the profile information for a user based on their username. The username is provided as a path parameter in the URL. If the username format is invalid, a Bad Request (400) response is returned. If the profile is not found, a Not Found (404) response is returned with a corresponding error message. If successful, the endpoint returns the profile information in the response body.

## CreateProfile

Creates a new user profile.

**Endpoint:** `/api/profile`

**Method:** POST

### Request

- Body: The profile object containing the following fields:
  - `Username` (string, required): The username for the new profile.
  - `Email` (string, required): The email address for the new profile.
  - `Password` (string, required): The password for the new profile.
  - `FirstName` (string, required): The first name for the new profile.
  - `LastName` (string, required): The last name for the new profile.

### Response

- Status: 201 Created
- Headers:
  - `Location`: The URL of the newly created profile resource.
- Body:
  - `Username` (string): The username of the created profile.
  - `Email` (string): The email address of the created profile.
  - `Password` (string): The password of the created profile.
  - `FirstName` (string): The first name of the created profile.
  - `LastName` (string): The last name of the created profile.

### Error Responses

- Status: 409 Conflict
  - Body: Indicates that a profile with the provided username or email already exists, and a new profile cannot be created. The error message provides additional details.
- Status: 400 Bad Request
  - Body: Indicates that the request body contains invalid data or is missing required fields.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows you to create a new user profile. The required user information, including username, email, password, first name, and last name, should be provided in the request body. Upon successful creation, a 201 Created response is returned along with the URL of the newly created profile resource in the `Location` header. If a profile with the same username or email already exists, a Conflict (409) response is returned with an error message. If the request body is invalid or missing required fields, a Bad Request (400) response is returned with details on the validation errors.

## Login

Logs in a user with their username or email and password.

**Endpoint:** `/api/profile/login`

**Method:** POST

### Request

- Body: The sign-in request object containing the following fields:
  - `LogInString` (string, required): The username or email of the user.
  - `Password` (string, required): The password for authentication.

### Response

- Status: 200 OK
- Body:
  - `Token` (string): The authentication token for the logged-in user.
  - `Expiration` (DateTime): The expiration date and time of the authentication token.
  - `Username` (string): The username of the logged-in user.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the provided password format is invalid.
- Status: 401 Unauthorized
  - Body: Indicates that the email or password is incorrect. Users are prompted to try again or sign up.
- Status: 404 Not Found
  - Body: Indicates that the provided username or email is invalid or not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to log in by providing their username or email along with their password. The required information is provided in the request body. If the password format is invalid, a Bad Request (400) response is returned. If the provided email or password is incorrect, a Unauthorized (401) response is returned with a corresponding error message, prompting users to try again or sign up. If the provided username or email is invalid or not found, a Not Found (404) response is returned. Upon successful login, a 200 OK response is returned along with the authentication token, expiration details, and the username of the logged-in user.

## VerifyProfile

Verifies a user's profile by confirming their email address.

**Endpoint:** `/api/profile/verify`

**Method:** POST

### Request

- `Username` (string, required): The username of the user whose profile is being verified.
- `VerificationCode` (string, required): The verification code sent to the user's email.

### Response

- Status: 201 Created
- Body: The verified user's profile information.

### Error Responses

- Status: 401 Unauthorized
  - Body: Indicates that the provided verification code is incorrect.
- Status: 404 Not Found
  - Body: Indicates that the specified profile with the provided username was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint is used to verify a user's profile by confirming their email address. The user is required to provide their username and the verification code received in their email. If the verification code is incorrect, a Unauthorized (401) response is returned. If the specified profile with the provided username is not found, a Not Found (404) response is returned. Upon successful verification, a 201 Created response is returned, and the verified user's profile information is provided in the response body.

## ResendVerificationEmail

Resends the email verification link to a user's email address.

**Endpoint:** `/api/profile/verify/{username}`

**Method:** GET

### Request

- `{username}` (path parameter): The username of the user for whom the verification email should be resent.

### Response

- Status: 200 OK
- Body: Indicates that the email verification link has been successfully resent to the user's email address.

### Error Responses

- Status: 404 Not Found
  - Body: Indicates that the specified user with the provided username was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to request a resend of the email verification link to their email address. The username of the user for whom the verification email should be resent is provided as a path parameter in the URL. If the provided username format is invalid, a Bad Request (400) response is returned. If the specified user with the provided username is not found, a Not Found (404) response is returned. Upon successful resend of the verification email, a 200 OK response is returned, indicating that the email has been successfully sent to the user's email address.

## UpdateProfile

Updates a user's profile information.

**Endpoint:** `/api/profile/{username}`

**Method:** PUT

### Request

- `{username}` (path parameter): The username of the user whose profile is being updated.
- Body: The `PutProfile` object containing the following fields to be updated:
  - `FirstName` (string, required): The new first name for the user's profile.
  - `LastName` (string, required): The new last name for the user's profile.

### Response

- Status: 200 OK
- Body: Indicates that the user's profile has been successfully updated.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the request body contains invalid data or is missing required fields.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to update this profile.
- Status: 404 Not Found
  - Body: Indicates that the specified user with the provided username was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to update their profile information, specifically the first name and last name. The username of the user whose profile is being updated is provided as a path parameter in the URL, and the updated information is provided in the request body as a `PutProfile` object. If the request body contains invalid data or is missing required fields, a Bad Request (400) response is returned with details on the validation errors. If the user is not authorized to update the profile, a Unauthorized (401) response is returned. If the specified user with the provided username is not found, a Not Found (404) response is returned. Upon successful update of the profile, a 200 OK response is returned, indicating that the user's profile has been successfully updated.

## DeleteProfile

Deletes a user's profile.

**Endpoint:** `/api/profile/{username}`

**Method:** DELETE

### Request

- `{username}` (path parameter): The username of the user whose profile is being deleted.

### Response

- Status: 200 OK
- Body: Indicates that the user's profile has been successfully deleted.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the provided username format is invalid.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to delete this profile.
- Status: 404 Not Found
  - Body: Indicates that the specified user with the provided username was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to delete their profile. The username of the user whose profile is being deleted is provided as a path parameter in the URL. If the provided username format is invalid, a Bad Request (400) response is returned. If the user is not authorized to delete the profile, a Unauthorized (401) response is returned. If the specified user with the provided username is not found, a Not Found (404) response is returned. Upon successful deletion of the profile, a 200 OK response is returned, indicating that the user's profile has been successfully deleted.

## ResetPasswordRequest

Initiates a password reset request for a user's profile.

**Endpoint:** `/api/profile/reset/{username}`

**Method:** POST

### Request

- `{username}` (path parameter): The username of the user for whom a password reset request is initiated.

### Response

- Status: 200 OK
- Body: Indicates that a password reset request has been successfully initiated for the user.

### Error Responses

- Status: 404 Not Found
  - Body: Indicates that the specified user with the provided username was not found.
- Status: 409 Conflict
  - Body: Indicates that a password reset request has already been sent to the user.

### Description

This endpoint allows users to initiate a password reset request for their profile. The username of the user for whom the request is initiated is provided as a path parameter in the URL. If the specified user with the provided username is not found, a Not Found (404) response is returned. If a password reset request has already been sent to the user, a Conflict (409) response is returned. Upon successful initiation of the password reset request, a 200 OK response is returned, indicating that the request has been successfully initiated for the user.

## ResendResetEmail

Resends the password reset email to a user's email address.

**Endpoint:** `/api/profile/reset/{username}`

**Method:** GET

### Request

- `{username}` (path parameter): The username of the user for whom the password reset email should be resent.

### Response

- Status: 200 OK
- Body: Indicates that the password reset email has been successfully resent to the user's email address.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the provided username format is invalid.
- Status: 404 Not Found
  - Body: Indicates that the password reset request for the specified user with the provided username was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to request a resend of the password reset email to their email address. The username of the user for whom the password reset email should be resent is provided as a path parameter in the URL. If the provided username format is invalid, a Bad Request (400) response is returned. If the password reset request for the specified user with the provided username is not found, a Not Found (404) response is returned. Upon successful resend of the password reset email, a 200 OK response is returned, indicating that the email has been successfully sent to the user's email address.


## ResetPassword

Resets a user's password based on a password reset request.

**Endpoint:** `/api/profile/reset`

**Method:** PUT

### Request

- Body: The `ChangedPasswordRequest` object containing the following fields:
  - `Username` (string, required): The username of the user for whom the password is being reset.
  - `Code` (string, required): The code associated with the password reset request.
  - `Password` (string, required): The new password for the user's profile.

### Response

- Status: 200 OK
- Body: Indicates that the user's password has been successfully reset.

### Error Responses

- Status: 404 Not Found
  - Body: Indicates that the password reset request for the specified user with the provided username was not found.
- Status: 400 Bad Request
  - Body: Indicates that the request body contains invalid data or is missing required fields.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to reset their password based on a password reset request. The required information is provided in the request body as a `ChangedPasswordRequest` object, which includes the username, the code associated with the password reset request, and the new password. If the password reset request for the specified user with the provided username is not found, a Not Found (404) response is returned. If the request body contains invalid data or is missing required fields, a Bad Request (400) response is returned with details on the validation errors. Upon successful password reset, a 200 OK response is returned, indicating that the user's password has been successfully reset.

</details>

<details>
<summary><h2>Friend API Documentation</h2></summary>
## GetFriends

Retrieves the list of friends for a user.

**Endpoint:** `/api/friends/{username}`

**Method:** GET

### Request

- `{username}` (path parameter): The username of the user for whom the list of friends is being retrieved.

### Response

- Status: 200 OK
- Body: A list of `Friend` objects representing the user's friends, each containing the following fields:
  - `Username` (string): The username of a friend.
  - `TimeAdded` (long): The timestamp indicating when the friend was added.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the provided username format is invalid.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to access the friend list.
- Status: 404 Not Found
  - Body: Indicates that the specified user with the provided username was not found or that no friends were found for the user.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to retrieve a list of their friends. The username of the user for whom the list of friends is being retrieved is provided as a path parameter in the URL. If the provided username format is invalid, a Bad Request (400) response is returned. If the user is not authorized to access the friend list, a Unauthorized (401) response is returned. If the specified user with the provided username is not found or if no friends were found for the user, a Not Found (404) response is returned. Upon successful retrieval of the friend list, a 200 OK response is returned, containing a list of `Friend` objects representing the user's friends, including their usernames and timestamps of when they were added.

## GetReceivedFriendRequests

Retrieves the list of received friend requests for a user.

**Endpoint:** `/api/friends/requests/{username}/received`

**Method:** GET

### Request

- `{username}` (path parameter): The username of the user for whom the list of received friend requests is being retrieved.

### Response

- Status: 200 OK
- Body: A list of `FriendRequest` objects representing the received friend requests, each containing the following fields:
  - `Sender` (string): The username of the sender of the friend request.
  - `Receiver` (string): The username of the receiver (current user) of the friend request.
  - `TimeAdded` (long): The timestamp indicating when the friend request was received.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the provided username format is invalid.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to access received friend requests.
- Status: 404 Not Found
  - Body: Indicates that the specified user with the provided username was not found or that no friend requests were found for the user.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to retrieve a list of received friend requests. The username of the user for whom the list of received friend requests is being retrieved is provided as a path parameter in the URL. If the provided username format is invalid, a Bad Request (400) response is returned. If the user is not authorized to access received friend requests, a Unauthorized (401) response is returned. If the specified user with the provided username is not found or if no friend requests were found for the user, a Not Found (404) response is returned. Upon successful retrieval of received friend requests, a 200 OK response is returned, containing a list of `FriendRequest` objects representing the received friend requests, including the sender's username, receiver's username, and timestamps of when they were received.

## GetSentFriendRequests

Retrieves the list of sent friend requests by a user.

**Endpoint:** `/api/friends/requests/{username}/sent`

**Method:** GET

### Request

- `{username}` (path parameter): The username of the user for whom the list of sent friend requests is being retrieved.

### Response

- Status: 200 OK
- Body: A list of `FriendRequest` objects representing the sent friend requests, each containing the following fields:
  - `Sender` (string): The username of the sender of the friend request.
  - `Receiver` (string): The username of the receiver of the friend request.
  - `TimeAdded` (long): The timestamp indicating when the friend request was sent.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the provided username format is invalid.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to access sent friend requests.
- Status: 404 Not Found
  - Body: Indicates that the specified user with the provided username was not found or that no sent friend requests were found for the user.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to retrieve a list of sent friend requests. The username of the user for whom the list of sent friend requests is being retrieved is provided as a path parameter in the URL. If the provided username format is invalid, a Bad Request (400) response is returned. If the user is not authorized to access sent friend requests, a Unauthorized (401) response is returned. If the specified user with the provided username is not found or if no sent friend requests were found for the user, a Not Found (404) response is returned. Upon successful retrieval of sent friend requests, a 200 OK response is returned, containing a list of `FriendRequest` objects representing the sent friend requests, including the sender's username, receiver's username, and timestamps of when they were sent.

## SendFriendRequest

Sends a friend request from one user to another.

**Endpoint:** `/api/friends/send`

**Method:** POST

### Request

- Body: A `CreateFriendRequest` object representing the friend request to be sent, containing the following fields:
  - `Sender` (string, required): The username of the sender of the friend request.
  - `Receiver` (string, required): The username of the receiver of the friend request.

### Response

- Status: 201 Created
- Headers: None
- Body: A success message indicating that the friend request has been sent, including the sender's username and the receiver's username.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the friend request is invalid or in an incorrect format.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to send the friend request.
- Status: 403 Forbidden
  - Body: Indicates that sending the friend request is not allowed due to certain conditions.
- Status: 404 Not Found
  - Body: Indicates that the user or the target user for the friend request was not found.
- Status: 409 Conflict
  - Body: Indicates that the sender has already sent a friend request to the receiver or that the sender and receiver are already friends.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to send friend requests to other users. The sender and receiver usernames are provided in the request body as part of the `CreateFriendRequest` object. If the friend request is invalid or in an incorrect format, a Bad Request (400) response is returned. If the user is not authorized to send the friend request, a Unauthorized (401) response is returned. If sending the friend request is not allowed due to certain conditions, a Forbidden (403) response may be returned. If the specified user or the target user for the friend request was not found, a Not Found (404) response is returned. If the sender has already sent a friend request to the receiver or if the sender and receiver are already friends, a Conflict (409) response is returned. Upon successful sending of the friend request, a 201 Created response is returned, including a success message indicating that the friend request has been sent, along with the sender's username and the receiver's username.

## AcceptFriendRequest

Accepts a friend request from one user to another, making them friends.

**Endpoint:** `/api/friends/accept`

**Method:** POST

### Request

- Body: A `FriendRequest` object representing the friend request to be accepted, containing the following fields:
  - `Sender` (string, required): The username of the sender of the friend request.
  - `Receiver` (string, required): The username of the receiver of the friend request.

### Response

- Status: 201 Created
- Headers: None
- Body: A success message indicating that the friend request has been accepted, including the sender's username and the receiver's username.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the friend request is invalid or in an incorrect format.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to accept the friend request.
- Status: 404 Not Found
  - Body: Indicates that the user or the friend request was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to accept a friend request from another user, making them friends. The sender and receiver usernames are provided in the request body as part of the `FriendRequest` object. If the friend request is invalid or in an incorrect format, a Bad Request (400) response is returned. If the user is not authorized to accept the friend request, a Unauthorized (401) response is returned. If the specified user or the friend request was not found, a Not Found (404) response is returned. Upon successful acceptance of the friend request, a 201 Created response is returned, including a success message indicating that the friend request has been accepted, along with the sender's username and the receiver's username.


## RemoveFriend

Removes a friend relationship between two users.

**Endpoint:** `/api/friends`

**Method:** DELETE

### Request

- Body: A `FriendRequest` object representing the friend relationship to be removed, containing the following fields:
  - `Sender` (string, required): The username of one of the friends.
  - `Receiver` (string, required): The username of the other friend.

### Response

- Status: 200 OK
- Headers: None
- Body: A success message indicating that the friend relationship has been removed, including the usernames of the friends.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the friend relationship request is invalid or in an incorrect format.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to remove the friend relationship.
- Status: 404 Not Found
  - Body: Indicates that the user or the friend relationship was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to remove a friend relationship between two users. The usernames of the two friends are provided in the request body as part of the `FriendRequest` object. If the friend relationship request is invalid or in an incorrect format, a Bad Request (400) response is returned. If the user is not authorized to remove the friend relationship, a Unauthorized (401) response is returned. If the specified user or the friend relationship was not found, a Not Found (404) response is returned. Upon successful removal of the friend relationship, a 200 OK response is returned, including a success message indicating that the friend relationship has been removed, along with the usernames of the friends.

## RemoveFriendRequest

Removes a friend request between two users.

**Endpoint:** `/api/friends/request`

**Method:** DELETE

### Request

- Body: A `FriendRequest` object representing the friend request to be removed, containing the following fields:
  - `Sender` (string, required): The username of the sender of the friend request.
  - `Receiver` (string, required): The username of the receiver of the friend request.

### Response

- Status: 200 OK
- Headers: None
- Body: A success message indicating that the friend request has been removed, including the usernames of the sender and receiver.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the friend request is invalid or in an incorrect format.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to remove the friend request.
- Status: 404 Not Found
  - Body: Indicates that the user or the friend request was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

### Description

This endpoint allows users to remove a friend request between two users. The usernames of the sender and receiver of the friend request are provided in the request body as part of the `FriendRequest` object. If the friend request is invalid or in an incorrect format, a Bad Request (400) response is returned. If the user is not authorized to remove the friend request, a Unauthorized (401) response is returned. If the specified user or the friend request was not found, a Not Found (404) response is returned. Upon successful removal of the friend request, a 200 OK response is returned, including a success message indicating that the friend request has been removed, along with the usernames of the sender and receiver.

</details>

<details>
<summary><h2>Image API Documentation</h2></summary>
The Images Controller handles the management of user profile images.

## UploadImage

**Endpoint:** `/api/images/{username}`

**Method:** POST

### Request

- Body: A binary image file to be uploaded as an `IFormFile`.
- Route Parameter:
  - `username` (string): The username of the user for whom the image is being uploaded.

### Response

- Status: 201 Created
- Headers: None
- Body: A success message indicating that the profile picture has been successfully posted.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the username format is invalid, the file type is not supported (PNG or JPEG), or the image size exceeds 5MB.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to upload an image.
- Status: 404 Not Found
  - Body: Indicates that the user with the specified username was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

## DownloadImage

**Endpoint:** `/api/images/{username}`

**Method:** GET

### Request

- Route Parameter:
  - `username` (string): The username of the user whose profile image is to be downloaded.

### Response

- Status: 200 OK
- Headers: `Content-Type` header indicating the content type of the image (e.g., "image/jpeg").
- Body: The binary image data.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the username format is invalid.
- Status: 404 Not Found
  - Body: Indicates that the profile picture for the specified username was not found.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

## DeleteImage

**Endpoint:** `/api/images/{username}`

**Method:** DELETE

### Request

- Route Parameter:
  - `username` (string): The username of the user for whom the profile image is to be deleted.

### Response

- Status: 200 OK
- Headers: None
- Body: A success message indicating that the profile picture has been successfully deleted.

### Error Responses

- Status: 404 Not Found
  - Body: Indicates that the profile picture for the specified username was not found.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to delete the profile image.
- Other Exceptions: The endpoint may throw various exceptions for unexpected errors.

## Description

The Images Controller allows users to upload, download, and delete profile images. When uploading an image, the username of the user is provided as a route parameter, and the image is validated for format (PNG or JPEG) and size. If successful, the image is stored. Downloading an image requires providing the username as a route parameter, and the image data is returned in the response. Deleting an image also requires the username as a route parameter and deletes the associated profile image.

Please note that authorization is enforced to ensure that only authorized users can perform these actions.

</details>

<details>
<summary><h2>Spotify API Documentation</h2></summary>
The Spotify Controller provides endpoints for interacting with the Spotify API to retrieve song and playlist information.

## GetSpotifyToken

**Endpoint:** `/api/spotify/token`

**Method:** GET

### Request

- None

### Response

- Status: 200 OK
- Body: A Spotify access token.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates a failure to retrieve the Spotify access token.

## GetTrack

**Endpoint:** `/api/spotify/song?songId={songId}&token={token}`

**Method:** GET

### Request

- Query Parameters:
  - `songId` (string): The unique identifier of the song.
  - `token` (string): The Spotify access token.

### Response

- Status: 200 OK
- Body: A `Song` object representing the song with details such as ID, name, image URL, and artists.

### Error Responses

- Status: 401 Unauthorized
  - Body: Indicates that the provided Spotify access token has expired.
- Status: 404 Not Found
  - Body: Indicates a failure to retrieve the song information.

## GetAlbum

**Endpoint:** `/api/spotify/album?albumId={albumId}&token={token}`

**Method:** GET

### Request

- Query Parameters:
  - `albumId` (string): The unique identifier of the album.
  - `token` (string): The Spotify access token.

### Response

- Status: 200 OK
- Body: A list of `Song` objects representing the songs in the specified album, including details such as ID, name, image URL, and artists.

### Error Responses

- Status: 401 Unauthorized
  - Body: Indicates that the provided Spotify access token has expired.
- Status: 404 Not Found
  - Body: Indicates a failure to retrieve the album's tracklist.

## GetPlaylist

**Endpoint:** `/api/spotify/playlist?playlistId={playlistId}&token={token}`

**Method:** GET

### Request

- Query Parameters:
  - `playlistId` (string): The unique identifier of the playlist.
  - `token` (string): The Spotify access token.

### Response

- Status: 200 OK
- Body: A list of `Song` objects representing the songs in the specified playlist, including details such as ID, name, image URL, and artists.

### Error Responses

- Status: 401 Unauthorized
  - Body: Indicates that the provided Spotify access token has expired.
- Status: 404 Not Found
  - Body: Indicates a failure to retrieve the playlist's tracklist.

## Description

The Spotify Controller allows users to obtain Spotify access tokens and retrieve song information from Spotify's catalog. The `GetSpotifyToken` endpoint retrieves a Spotify access token, which can be used for subsequent requests to the Spotify API. The other endpoints (`GetTrack`, `GetAlbum`, and `GetPlaylist`) allow users to retrieve song information based on the song, album, or playlist ID, respectively, using the Spotify access token.

Please note that the endpoints may return error responses in case of token expiration or failure to retrieve song/playlist information from Spotify.
</details>
<details>
<summary><h2>Feed API Documentation</h2></summary>

The Feed Controller provides endpoints for managing user posts and retrieving posts from friends.

## AddPost (Spotify)

**Endpoint:** `/api/feed/spotify`

**Method:** POST

### Request

- Body: `PostRequest` object containing the following properties:
  - `Username` (string, required): The username of the user posting.
  - `ContentId` (string, required): The unique identifier of the content (e.g., a Spotify track) being posted.

### Response

- Status: 200 OK
- Body: A new `Post` object representing the created post.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the provided username is in an invalid format.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to perform this action.
- Status: 404 Not Found
  - Body: Indicates that the user's profile was not found.
- Status: 409 Conflict
  - Body: Indicates that a post with the same content already exists.

## DeletePost

**Endpoint:** `/api/feed`

**Method:** DELETE

### Request

- Body: `DeletePostRequest` object containing the following properties:
  - `Username` (string, required): The username of the user whose post is to be deleted.
  - `Platform` (string, required): The platform associated with the post (e.g., "spotify").

### Response

- Status: 200 OK
- Body: Indicates that the post was successfully deleted.

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the request is invalid (e.g., missing required properties).
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to perform this action.
- Status: 404 Not Found
  - Body: Indicates that the post was not found.
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to perform this action.

## GetPostsOfFriends

**Endpoint:** `/api/feed?username={username}&pageNumber={pageNumber}&pageSize={pageSize}`

**Method:** GET

### Request

- Query Parameters:
  - `username` (string, required): The username of the user whose friend posts are to be retrieved.
  - `pageNumber` (int, optional): The page number for paginated results (default is 1).
  - `pageSize` (int, optional): The number of posts per page (default is 12).

### Response

- Status: 200 OK
- Body: An `EnumeratePostResponse` object containing a list of `Post` objects representing friend posts and a `NextURI` string that provides the URI for the next page of results (if available).

### Error Responses

- Status: 400 Bad Request
  - Body: Indicates that the request is invalid (e.g., invalid username format or page number/page size less than 1).
- Status: 401 Unauthorized
  - Body: Indicates that the user is not authorized to perform this action.
- Status: 404 Not Found
  - Body: Indicates that no more friend posts were found for the user.

## Description

The Feed Controller allows users to create and delete posts related to content from various platforms (e.g., Spotify). Users can add a new post (`AddPost`) associated with their username and content ID. Posts can be deleted (`DeletePost`) based on the username and platform. Users can also retrieve posts from friends (`GetPostsOfFriends`) in a paginated manner.

Please note that the endpoints may return error responses in case of invalid requests, unauthorized access, post not found, or other exceptional situations.

</details>
