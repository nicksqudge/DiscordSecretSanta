# Commands

| Command       | Description                                                    | Permission                 | Version |
|---------------|----------------------------------------------------------------|----------------------------|---------|
| join          | Request to join an on going secret santa                       | People not in secret santa | v1      |
| add @username | Toggles a user as an admin                                     | Admin                      | v1      |
| sent          | Mark that you have sent your secret santa their gift           | People in secret santa     | N/A     |
| recieved      | Mark that you have recieved your secret santa gift             | People in secret santa     | N/A     |
| open          | Opens a secret santa for people to join                        | Admin                      | v1      |
| draw          | Draws the names for secret santa and closes new people to join | Admin                      | v1      |
| status        | Displays the status of the secret santa                        | Anyone                     | v1      |
| close         | Closes the secret santa without drawing                        | Admin                      | N/A     |
| max-price     | Sets the max price for the campaign (only before draw)         | Admin                      | v1      |
| help          | Provides a link to the github and the version number           | Anyone                     | N/A     |
| who           | Find out who you drew in secret santa (only done after draw)   | People in secret santa     | v1      |