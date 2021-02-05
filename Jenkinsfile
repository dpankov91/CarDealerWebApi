pipeline{
	agent any
		triggers {
		 cron("0 * * * *")
}
stages {
	stage("Build") {
	  steps {
	    sh "dotnet build BookStore-BackEnd/BookStore-BackEnd.csproj"
		}
}
	stage("Test") {
	  steps {
	    sh "dotnet build BookStore-BackEnd/BookStore-BackEnd.csproj"
		}
	}
}
}
	