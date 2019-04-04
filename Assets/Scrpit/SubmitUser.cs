using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SubmitUser : MonoBehaviour
{
    //Variables needed
    public Text toolTip;
    public InputField username, password, email;

    private EventSystem system;

    public void Submit()
    {
        //Put this in a public function
        StartCoroutine(CreateUser(username.text, password.text, email.text));
    }
    IEnumerator CreateUser(string _username, string _password, string _email)
    {
        //Link to PHP
        string createUserURL = "http://localhost/squealsystem/InsertUser.php";

        //Info to send to the POST variables in PHP
        WWWForm insetUserForm = new WWWForm();
        insetUserForm.AddField("usernamePost", _username);
        insetUserForm.AddField("passwordPost", _password);
        insetUserForm.AddField("emailPost", _email);

        //Send info to PHP
        WWW www = new WWW(createUserURL, insetUserForm);

        //get feedback and display tool tip
        yield return www;
        toolTip.text = www.text;
        Debug.Log(www.text);
    }
    
    private void Start()
    {
        system = EventSystem.current;
    }
    private void Update()
    {
        if (system.currentSelectedGameObject == null || !Input.GetKeyDown(KeyCode.Tab))
            return;

        Selectable current = system.currentSelectedGameObject.GetComponent<Selectable>();
        if (current == null)
            return;

        bool up = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        Selectable next = up ? current.FindSelectableOnUp() : current.FindSelectableOnDown();

        // We are at the end or the beginning, go to either, depends on the direction we are tabbing in
        // The previous version would take the logical 0 selector, which would be the highest up in your editor hierarchy
        // But not certainly the first item on your GUI, or last for that matter
        // This code tabs in the correct visual order
        if (next == null)
        {
            next = current;

            Selectable pnext;
            if (up) while ((pnext = next.FindSelectableOnDown()) != null) next = pnext;
            else while ((pnext = next.FindSelectableOnUp()) != null) next = pnext;
        }

        // Simulate Inputfield MouseClick
        InputField inputfield = next.GetComponent<InputField>();
        if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));

        // Select the next item in the taborder of our direction
        system.SetSelectedGameObject(next.gameObject);
    }
}
