---
title: Button
route: /Button
---

# My Counter Page

```razor
@page "/MyCounter"

Value: @Value

<button @onclick="Button_Click">Click</button>

@code {
    private int Value = 0;

    private void Button_Click()
    {
        Value++;
    }
}
```

## Demo

{{ MyCounter }}

This code is a live demo of a Counter

|Example|Table|
|---|---|
|1.1|1.2|
|2.1|2.2|

> My comment

{{ INVALID }}

```html
<div style="color: red;">
  Hello World
</div>
```

```js
function hello() {
  console.log('Hello World');
}
```