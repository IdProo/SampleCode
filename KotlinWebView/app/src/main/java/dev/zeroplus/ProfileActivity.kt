package dev.zeroplus

import android.content.SharedPreferences
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.ImageView
import android.widget.TextView
import com.bumptech.glide.Glide
import com.bumptech.glide.request.RequestOptions
import com.google.gson.Gson
import dev.zeroplus.models.User

class ProfileActivity : AppCompatActivity() {
    private lateinit var shared: SharedPreferences
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_profile)
        shared = getSharedPreferences("office",0)

        val actionbar = supportActionBar
        //set actionbar title
        actionbar!!.title = "Profile"
        //set back button
        actionbar.setDisplayHomeAsUpEnabled(true)
        val user: User = Gson().fromJson<User>(shared.getString("user","{}"), User::class.java)
        val xx: String = if(user.companyName is String) user.companyName else user.companyCode

        findViewById<TextView>(R.id.tvEmail).text = user.email
        findViewById<TextView>(R.id.tvDisplayName).text = user.displayName
        findViewById<TextView>(R.id.tvPhoneNumber).text = user.mobilePhone
        findViewById<TextView>(R.id.tvCompany).text = xx
        val ivProfile: ImageView = findViewById<ImageView>(R.id.ivProfile)

        val options: RequestOptions = RequestOptions()
            .centerCrop()
            .circleCrop()
            .placeholder(R.mipmap.ic_launcher_round)
            .error(R.mipmap.ic_launcher_round)

        //Glide.with(this)
        //    .load("https://photo/${user.userId}")
        //    .apply(options).into(ivProfile)
    }

    override fun onSupportNavigateUp(): Boolean {
        onBackPressed()
        return true
    }
}