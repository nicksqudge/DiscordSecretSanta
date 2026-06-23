# Commands

| Command       | Context | Description                                                    | Permission                 | Version |
|---------------|---------|----------------------------------------------------------------|----------------------------|---------|
| join          | Guild   | Request to join an on going secret santa                       | People not in secret santa | v1      |
| add @username | Any     | Toggles a user as an admin                                     | Admin                      | v1      |
| sent          | DM      | Mark that you have sent your secret santa their gift           | People in secret santa     | N/A     |
| recieved      | DM      | Mark that you have recieved your secret santa gift             | People in secret santa     | N/A     |
| open          | Guild   | Opens a secret santa for people to join                        | Admin                      | v1      |
| draw          | Guild   | Draws the names for secret santa and closes new people to join | Admin                      | v1      |
| status        | Guild   | Displays the status of the secret santa                        | Anyone                     | v1      |
| close         | Guild   | Closes the secret santa without drawing                        | Admin                      | N/A     |
| max-price     | Guild   | Sets the max price for the campaign (only before draw)         | Admin                      | v1      |
| help          | Any     | Provides a link to the github and the version number           | Anyone                     | N/A     |
| who           | Any     | Find out who you drew in secret santa (only done after draw)   | People in secret santa     | v1      |