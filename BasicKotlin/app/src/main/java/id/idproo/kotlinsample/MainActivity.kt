package id.idproo.kotlinsample

import android.content.Intent
import android.net.Uri
import android.os.AsyncTask
import android.os.Bundle
import android.text.Editable
import android.util.Log
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.LinearLayout
import androidx.appcompat.app.AppCompatActivity
import androidx.core.net.toUri
import net.openid.appauth.*
import okhttp3.OkHttpClient
import okhttp3.Request
import org.json.JSONObject


class MainActivity : AppCompatActivity() {

    private val RC_AUTH = 100
    private val RC_END_SESSION = 200

    private var serviceConfig = AuthorizationServiceConfiguration(
        Uri.parse("${Shared.oauth.issuer}/connect/authorize"),
        Uri.parse("${Shared.oauth.issuer}/connect/token"),
        null,
        Uri.parse("${Shared.oauth.issuer}/connect/endsession")
    )

    private lateinit var etAccessToken: EditText
    private lateinit var etIdToken: EditText
    private lateinit var etRefreshToken: EditText
    private lateinit var etUserInfo: EditText
    private lateinit var sectionToken: LinearLayout
    private lateinit var loginButton: Button
    private lateinit var logoutButton: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        loginButton = findViewById(R.id.btn_login)
        logoutButton = findViewById(R.id.btn_logout)
        etAccessToken = findViewById(R.id.et_access_token)
        etIdToken = findViewById(R.id.et_id_token)
        etUserInfo= findViewById(R.id.et_user_info)
        etRefreshToken = findViewById(R.id.et_refresh_token)

        sectionToken = findViewById(R.id.section_token)

        sectionToken.visibility = View.GONE

        loginButton.setOnClickListener {
            authorize()
        }

        logoutButton.setOnClickListener {
            endSession(etIdToken.text.toString())
        }
    }

    private fun endSession(idToken: String) {
        val endSessionRedirect = Shared.oauth.endSessionRedirect.toUri()
        val endSessionRequest = EndSessionRequest.Builder(serviceConfig)
            .setIdTokenHint(idToken)
            .setPostLogoutRedirectUri(endSessionRedirect)
            .build()
        val authService = AuthorizationService(this)
        val endSessionItent = authService.getEndSessionRequestIntent(endSessionRequest)
        startActivityForResult(endSessionItent, RC_END_SESSION)
    }


    private fun authorize() {
        val authRequestBuilder = AuthorizationRequest.Builder(
            serviceConfig,
            Shared.oauth.clientId,
            ResponseTypeValues.CODE,
            Uri.parse(Shared.oauth.redirect)
        )

        val authRequest = authRequestBuilder
            .setScope("openid email profile offline_access")
            .setLoginHint("fulan.ahmad@idproo.id")
            .build()

        val authService = AuthorizationService(this)
        val authIntent = authService.getAuthorizationRequestIntent(authRequest)
        startActivityForResult(authIntent, RC_AUTH)
    }


    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == RC_AUTH && data is Intent) {
            val resp: AuthorizationResponse? = AuthorizationResponse.fromIntent(data)
            if (resp != null) {
                val authService = AuthorizationService(this)
                authService.performTokenRequest(
                    resp.createTokenExchangeRequest()
                ) { response, _ ->
                    if (response != null) {
                        sectionToken.visibility = View.VISIBLE
                        Log.d("OAUTH", "Response ${response.accessToken}")
                        etAccessToken.text = "${response.accessToken}".toEditable()
                        etIdToken.text = "${response.idToken}".toEditable()
                        etRefreshToken.text = "${response.refreshToken}".toEditable()
                        logoutButton.visibility = View.VISIBLE
                        loginButton.visibility = View.GONE
                        ProfileTask().execute(response.accessToken)
                    }
                }
            }
        } else if (requestCode == RC_END_SESSION && data is Intent) {
            Log.d("OAUTH", "OnLogout ${data}")
            etAccessToken.text.clear()
            etIdToken.text.clear()
            etRefreshToken.text.clear()
            logoutButton.visibility = View.GONE
            loginButton.visibility = View.VISIBLE

            sectionToken.visibility = View.GONE

        }

    }

    fun String.toEditable(): Editable = Editable.Factory.getInstance().newEditable(this)

    inner class ProfileTask: AsyncTask<String?, Void, JSONObject>() {
        override fun doInBackground(vararg tokens: String?): JSONObject? {
            val client = OkHttpClient()
            val request = Request.Builder()
                .url("${Shared.oauth.issuer}/connect/userinfo")
                .addHeader("Authorization", String.format("Bearer %s", tokens[0]))
                .build()
            try {
                val response = client.newCall(request).execute()
                val jsonBody: String = response.body()!!.string()
                Log.i("LOG_TAG", String.format("User Info Response %s", jsonBody))
                return JSONObject(jsonBody)
            } catch (exception: Exception) {
                Log.w("LOG_TAG", exception)
            }
            return null
        }

        override fun onPostExecute(userInfo: JSONObject?) {
            if (userInfo != null) {
                val fullName = userInfo.optString("name", null)
                etUserInfo.text = userInfo.toString().toEditable()
                Log.d("OAUTH","fullname ${fullName} ${userInfo}")
            }
        }
    }
}