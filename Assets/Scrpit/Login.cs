using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Login : MonoBehaviour {

    public Text toolTip;
    public InputField username, password, email;

    private EventSystem system;
    // Use this for initialization
    void Start () {

        system = EventSystem.current;
    }

    public void CallLogin()
    {
        StartCoroutine(FillLogin(username.text, password.text));
    }
    IEnumerator FillLogin(string _username, string _password)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", _username);
        form.AddField("passwordPost", _password);

        WWW www = new WWW("http://localhost/squealsystem/Login.php", form);
        yield return www;
        toolTip.text = www.text;
        Debug.Log(www.text);
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
