### Placeholders and autofill
The `Placeholder` parameter is used to set the placeholder text for the input field. This is a short hint that describes the expected value of the input field.
It is displayed when the input field is empty and not focused. 

The placeholder value affects the autofill suggestion feature in Microsoft Edge and Google Chrome.
Even if you set the `Autofill` parameter to `off`, the browser may still display autofill suggestions based on the placeholder value.

There are certain placeholder values which you should avoid to prevent the browser from showing autofill suggestions.

| Value (Placeholder/Name) | Description                          |
|--------------------------|--------------------------------------|
| `name`, `full name`, `first name`, `last name` | Personal name fields                   |
| `email`, `e-mail`, `mail`                | Email address fields                   |
| `address`, `street`, `city`, `zip`, `postal` | Address and postal code fields        |
| `phone`, `tel`                           | Phone number fields                    |
| `username`, `user`, `login`             | Username or login fields              |
| `password`, `pwd`                       | Password fields                        |
| `dob`, `birthdate`, `date of birth`     | Date of birth fields                   |
| `cc`, `card number`, `credit card`      | Credit card fields                     |

If you still want to use these placeholder values, then you need to disable autofill in your browser settings completely.
