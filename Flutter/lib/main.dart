import 'dart:io' show Platform;
import 'package:flutter/material.dart';
import 'package:flutter_appauth/flutter_appauth.dart';
import 'package:url_launcher/url_launcher.dart';

void main() => runApp(MyApp());

class MyApp extends StatefulWidget {
  @override
  _MyAppState createState() => _MyAppState();
}

class _MyAppState extends State<MyApp> {
  bool _isBusy = false;
  final FlutterAppAuth _appAuth = FlutterAppAuth();
  String? _codeVerifier;
  String? _authorizationCode;
  String? _refreshToken;
  String? _accessToken;
  TextEditingController _authorizationCodeTextController =
      TextEditingController();
  TextEditingController _accessTokenTextController = TextEditingController();
  TextEditingController _accessTokenExpirationTextController =
      TextEditingController();

  TextEditingController _idTokenTextController = TextEditingController();
  TextEditingController _refreshTokenTextController = TextEditingController();

  String _clientId = 'client_id';
  String _redirectUrl = 'com.example.fluttersso:/oauthredirect';
  String _issuer = 'https://login.provider.com';
  List<String> _scopes = <String>[
    'openid',
    'profile',
    'email',
    'offline_access'
  ];

  AuthorizationServiceConfiguration _serviceConfiguration =
      const AuthorizationServiceConfiguration(
    authorizationEndpoint: 'https://login.provider.com/connect/authorize',
    tokenEndpoint: 'https://login.provider.com/connect/token',
    endSessionEndpoint: 'https://login.provider.com/connect/endsession',
  );

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        appBar: AppBar(
          title: const Text('Plugin example app'),
        ),
        body: SingleChildScrollView(
          child: Column(
            children: <Widget>[
              Visibility(
                visible: _isBusy,
                child: const LinearProgressIndicator(),
              ),
              ElevatedButton(
                child: const Text('Sign in with no code exchange'),
                onPressed: _signInWithNoCodeExchange,
              ),
              ElevatedButton(
                child: const Text('Exchange code'),
                onPressed: _authorizationCode != null ? _exchangeCode : null,
              ),
              ElevatedButton(
                child: const Text('Sign in with auto code exchange'),
                onPressed: () => _signInWithAutoCodeExchange(),
              ),
              if (Platform.isIOS)
                Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: ElevatedButton(
                    child: const Text(
                      'Sign in with auto code exchange using ephemeral session (iOS only)',
                      textAlign: TextAlign.center,
                    ),
                    onPressed: () => _signInWithAutoCodeExchange(
                        preferEphemeralSession: true),
                  ),
                ),
              ElevatedButton(
                child: const Text('Refresh token'),
                onPressed: _refreshToken != null ? _refresh : null,
              ),
              const Text('authorization code'),
              TextField(
                controller: _authorizationCodeTextController,
              ),
              const Text('access token'),
              TextField(
                controller: _accessTokenTextController,
              ),
              const Text('access token expiration'),
              TextField(
                controller: _accessTokenExpirationTextController,
              ),
              const Text('id token'),
              TextField(
                controller: _idTokenTextController,
              ),
              const Text('refresh token'),
              TextField(
                controller: _refreshTokenTextController,
              ),
              ElevatedButton(
                onPressed: () async {
                  // sharePreferences clear/empty
                  // hive clear/empty
                  // SQlite clear/empty
                  // launch("https://identity.urinya/logout");
                  await _appAuth.endSession(EndSessionRequest(
                      idTokenHint: _idTokenTextController.text,
                      postLogoutRedirectUrl:
                          'com.example.fluttersso:/',
                      serviceConfiguration: _serviceConfiguration));

                  _authorizationCodeTextController.clear();
                  _accessTokenExpirationTextController.clear();
                  _accessTokenTextController.clear();
                  _authorizationCodeTextController.clear();
                  _accessTokenTextController.clear();
                  _idTokenTextController.clear();
                  _refreshTokenTextController.clear();
                },
                child: Text('Logout'),
              )
            ],
          ),
        ),
      ),
    );
  }

  Future<void> _refresh() async {
    try {
      _setBusyState();
      final TokenResponse? result = await _appAuth.token(TokenRequest(
          _clientId, _redirectUrl,
          refreshToken: _refreshToken,
          discoveryUrl: '$_issuer/.well-known/openid-configuration',
          scopes: _scopes));
      _processTokenResponse(result);
      // await _testApi(result);
    } catch (_) {
      _clearBusyState();
    }
  }

  Future<void> _exchangeCode() async {
    try {
      _setBusyState();
      final TokenResponse? result = await _appAuth.token(TokenRequest(
          _clientId, _redirectUrl,
          authorizationCode: _authorizationCode,
          discoveryUrl: '$_issuer/.well-known/openid-configuration',
          codeVerifier: _codeVerifier,
          scopes: _scopes));
      _processTokenResponse(result);
      // await _testApi(result);
    } catch (_) {
      _clearBusyState();
    }
  }

  Future<void> _signInWithNoCodeExchange() async {
    try {
      _setBusyState();
      // use the discovery endpoint to find the configuration
      final AuthorizationResponse? result = await _appAuth.authorize(
        AuthorizationRequest(_clientId, _redirectUrl,
            discoveryUrl: '$_issuer/.well-known/openid-configuration',
            scopes: _scopes,
            loginHint: 'bob'),
      );

      // or just use the issuer
      // var result = await _appAuth.authorize(
      //   AuthorizationRequest(
      //     _clientId,
      //     _redirectUrl,
      //     issuer: _issuer,
      //     scopes: _scopes,
      //   ),
      // );
      if (result != null) {
        _processAuthResponse(result);
      }
    } catch (_) {
      _clearBusyState();
    }
  }

  Future<void> _signInWithAutoCodeExchange(
      {bool preferEphemeralSession = false}) async {
    try {
      _setBusyState();

      // show that we can also explicitly specify the endpoints rather than getting from the details from the discovery document
      final AuthorizationTokenResponse? result =
          await _appAuth.authorizeAndExchangeCode(
        AuthorizationTokenRequest(
          _clientId,
          _redirectUrl,
          serviceConfiguration: _serviceConfiguration,
          scopes: _scopes,
          preferEphemeralSession: preferEphemeralSession,
        ),
      );

      // this code block demonstrates passing in values for the prompt parameter. in this case it prompts the user login even if they have already signed in. the list of supported values depends on the identity provider
      // final AuthorizationTokenResponse result = await _appAuth.authorizeAndExchangeCode(
      //   AuthorizationTokenRequest(_clientId, _redirectUrl,
      //       serviceConfiguration: _serviceConfiguration,
      //       scopes: _scopes,
      //       promptValues: ['login']),
      // );

      if (result != null) {
        _processAuthTokenResponse(result);
        // await _testApi(result);
      }
    } catch (_) {
      _clearBusyState();
    }
  }

  void _clearBusyState() {
    setState(() {
      _isBusy = false;
    });
  }

  void _setBusyState() {
    setState(() {
      _isBusy = true;
    });
  }

  void _processAuthTokenResponse(AuthorizationTokenResponse response) {
    setState(() {
      _accessToken = _accessTokenTextController.text = response.accessToken!;
      _idTokenTextController.text = response.idToken!;
      _refreshToken = _refreshTokenTextController.text = response.refreshToken!;
      _accessTokenExpirationTextController.text =
          response.accessTokenExpirationDateTime!.toIso8601String();
    });
  }

  void _processAuthResponse(AuthorizationResponse response) {
    setState(() {
      // save the code verifier as it must be used when exchanging the token
      _codeVerifier = response.codeVerifier;
      _authorizationCode =
          _authorizationCodeTextController.text = response.authorizationCode!;
      _isBusy = false;
    });
  }

  void _processTokenResponse(TokenResponse? response) {
    setState(() {
      _accessToken = _accessTokenTextController.text = response!.accessToken!;
      _idTokenTextController.text = response.idToken!;
      _refreshToken = _refreshTokenTextController.text = response.refreshToken!;
      _accessTokenExpirationTextController.text =
          response.accessTokenExpirationDateTime!.toIso8601String();
    });
  }
}
