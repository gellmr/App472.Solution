module.exports = function(grunt) {

  // Project configuration.
  grunt.initConfig({

    pkg: grunt.file.readJSON('package.json'),
    
    clean: {
      // Wipe everything in our Deploy.to.wwwroot folder
      Content:['./Deploy.to.wwwroot/**/*']
    }
  });

  // Load externally defined tasks. 
  grunt.loadNpmTasks('grunt-contrib-clean');
 
  grunt.loadNpmTasks('grunt-contrib-copy');

  // Establish tasks we can run from the terminal.
 
  grunt.registerTask('default', ['clean', 'copy']);

};