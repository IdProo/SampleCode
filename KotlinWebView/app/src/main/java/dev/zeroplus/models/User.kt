package dev.zeroplus.models

import com.google.gson.annotations.SerializedName

data class User(
    @SerializedName("id")
    val id: String,
    @SerializedName("userId")
    val userId: String,
    @SerializedName("position")
    val position: Position,
    @SerializedName("companyCode")
    val companyCode: String,
    @SerializedName("companyName")
    val companyName: String,
    @SerializedName("displayName")
    val displayName: String,
    @SerializedName("employeeId")
    val employeeId: String,
    @SerializedName("jobTitle")
    val jobTitle: String,
    @SerializedName("email")
    val email: String,
    @SerializedName("mobilePhone")
    val mobilePhone: String,
    @SerializedName("photo")
    val photo: String,
    @SerializedName("employeeNumber")
    val employeeNumber: String,
    @SerializedName("employeeType")
    val employeeType: String,
    @SerializedName("cultureInfo")
    val cultureInfo: String,
    @SerializedName("language")
    val language: String,
    @SerializedName("dateFormat")
    val dateFormat: String,
    @SerializedName("timeFormat")
    val timeFormat: String,
    @SerializedName("isCheif")
    val isCheif: Boolean
)