#include "stdafx.h"
#include <stdlib.h>
#include <string>
#include <iostream>
#include <stdio.h>
#include <sstream>
#include <algorithm>
#include <iterator>
#include <vector>
#include <set>
#include <unordered_set>
#include <direct.h>
#include <assert.h>
#include <fstream>

using namespace std;

string exec(const char* cmd) {
    
	FILE* pipe = _popen(cmd, "r");
    
	if (!pipe) return "ERROR";
    
	char buffer[300];
    string result = "";
    
	while(!feof(pipe)) {
        if(fgets(buffer, 300, pipe) != NULL)
                result += buffer;
    }

    _pclose(pipe);
    return result;
}

void custom_replace(string& text, const string searchString, const string replaceString)
{
    assert( searchString != replaceString );

    string::size_type pos = 0;

    while ( (pos = text.find(searchString, pos)) != string::npos ) 
	{
        text.replace( pos, searchString.size(), replaceString );
        pos++;
    }
}

const char* node_types[] =
{
    "null", "document", "element", "pcdata", "cdata", "comment", "pi", "declaration"
};

struct simple_walker: pugi::xml_tree_walker
{
	simple_walker(string node,string value,bool append) : _node(node), _value(value), _append(append)
	{
	}

    virtual bool for_each(pugi::xml_node& node)
    {
		if (string(node.name()) == _node)
		{
		   pugi::xml_node child = node.first_child();

		   string new_value="";
		   if (_append)
		   new_value = child.value() + _value;
		   else new_value = _value;
			   
		   child.set_value(new_value.c_str());
		}

        return true; // continue traversal
    }

	string _node;
	string _value;
	bool _append;
};

struct simple_walker_add: pugi::xml_tree_walker
{
	simple_walker_add(string node,string new_node_name,string new_node_value) : _node(node), _new_node_name(new_node_name), _new_node_value(new_node_value)
	{
	}

    virtual bool for_each(pugi::xml_node& node)
    {
		if (string(node.name()) == _node)
		{
			pugi::xml_node param = node.append_child(_new_node_name.c_str());
			param.append_child(pugi::node_pcdata).set_value(_new_node_value.c_str());
		}

        return true; // continue traversal
    }

	string _node;
	string _new_node_name;
	string _new_node_value;
};

struct replace_value_walker: pugi::xml_tree_walker
{
	string _node;
	string _search_value;
	string _new_value;

	replace_value_walker(string node,string search_value,string new_value) : _node(node), _search_value(search_value), _new_value(new_value)
	{
	}

    virtual bool for_each(pugi::xml_node& node)
    {
		if (string(node.name()) == _node)
		{
		   pugi::xml_node child = node.first_child();

		   string v = string(child.value());
		   custom_replace(v,_search_value,_new_value);
		   child.set_value(v.c_str());
		}

        return true; // continue traversal
    }
};

string get_global_openvibe_includes(string openvibe_base)
{
	string result="";

	result+=openvibe_base+"/openvibe/trunc/include;";
    result+=openvibe_base+"/openvibe-toolkit/trunc/src;";
    result+=openvibe_base+"/cmake-modules;";
    result+=openvibe_base+"/openvibe-modules/ebml/trunc/include;";
    result+=openvibe_base+"/openvibe-toolkit/trunc/include/openvibe-toolkit/tools;";
    result+=openvibe_base+"/openvibe-modules/system/trunc/include;";
    result+=openvibe_base+"/openvibe-modules/xml/trunc/include;";
	result+=openvibe_base+"/openvibe-modules/fs/trunc/include;";
	result+=openvibe_base+"/openvibe-modules/automaton/trunc/include;";
	result+=openvibe_base+"/openvibe-modules/socket/trunc/include;";
	result+=openvibe_base+"/openvibe-modules/stream/trunc/include;";
	result+=openvibe_base+"/dependencies/boost/include;";
	result+=openvibe_base+"/dependencies/openal/include;";
	result+=openvibe_base+"/openvibe/trunc/include/openvibe;";
    result+=openvibe_base+"/openvibe-toolkit/trunc/include/openvibe-toolkit;";
	result+=openvibe_base+"/dependencies/vrpn/include;";

	return result;
}

string get_openvibe_executable(const string full_path)
{
	 int n =  full_path.find_last_of("/");

	 string executable;
	 executable.assign(full_path,n+1,full_path.size()-n); 
	 custom_replace(executable,".vcxproj","");
	 return executable + ".exe";
}

string get_openvibe_base_folder(const string full_path)
{
	 int n =  full_path.find_last_of("/");
	 string folder;
	 
	 folder.assign(full_path,0,n+1); 
	 folder = folder+"../../..";
	 return folder;
}

bool file_exists(const char *filename)
{
    ifstream ifile(filename);
    return ifile;
}

void create_vs_user_file(const string path)
{
	 cout<<"Creating a new file:"+path<<endl;
	 ofstream myfile;
	 myfile.open (path.c_str());
     myfile << "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
	 myfile << "<Project xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">\r\n";
	 myfile << "</Project>\r\n";
     myfile.close();
}

int _tmain(int argc, TCHAR* argv[])
{
	//std::cin.get();
	cout<<"Prerequisites:"<<endl;
	cout<<"1) You must have dependencies installed"<<endl;
	cout<<"2) OpenVibe compiled using provided in \\scripts bat file"<<endl;
	cout<<"3) You must have VS projects generated using bat file from \\scripts"<<endl<<endl;
	cout<<"Please close ALL instances of Visual Studio!"<<endl<<endl;

	string project = "";
	if (argc>1) project = string(argv[1]);
	else 
	{
		cout<<"Usage:"<<endl<<"AdjustOpenVibe.exe \"D:\\Data\\current_work\\OpenVibe_src\\local-tmp\\visual\\trunc\\OpenViBE-acquisition-server-dynamic.vcxproj\""<<endl;
		return 0;
	}

	if(!file_exists(project.c_str()))
	{
		cout<<"File not found! Aborting...";
	    return 0;
	}

	custom_replace(project,"\\","/");
	cout<<project<<endl;

	string openvibe_base = get_openvibe_base_folder(project);

    string executable = get_openvibe_executable(project);
	cout<<"Executable="<<executable<<endl;

	//1. Set Additional Include folders
	cout<<"1. Setting Additional Include Folders"<<endl;
	
	pugi::xml_document doc;

	pugi::xml_parse_result result = doc.load_file(project.c_str());
	cout<<result.description()<<endl;

	string includes = ";"+get_global_openvibe_includes(openvibe_base);
    simple_walker walker("AdditionalIncludeDirectories",includes,true);
    doc.traverse(walker);
	
	//2. Setting Additional Libraries Folders
	cout<<"2. Setting Additional Libraries Folders"<<endl;
	string libs = ";"+openvibe_base+"/dependencies/boost/lib";
    walker = simple_walker("AdditionalLibraryDirectories",libs,true);
    doc.traverse(walker);

	//3. Setting Multiprocessor compilation
	cout<<"3. Setting Multiprocessor compilation"<<endl;
	simple_walker_add walker_add("ClCompile","MultiProcessorCompilation","true");
	doc.traverse(walker_add);

	//remove TARGET_HAS_ThirdPartyEmotivAPI
	cout<<"Emotiv driver disabled!"<<endl;
    replace_value_walker replace_walker = replace_value_walker("PreprocessorDefinitions","TARGET_HAS_ThirdPartyEmotivAPI;","");
    doc.traverse(replace_walker);

	doc.save_file(project.c_str());
	cout<<"Done."<<endl;
	
	//4. Change output directory
	cout<<"4. Set output folder to dist folder"<<endl;
	walker = simple_walker("OutDir","..\\..\\..\\dist\\bin",false);
    doc.traverse(walker);
	
	doc.save_file(project.c_str());
	cout<<"Done."<<endl;

	//5. Open .user file and set variables
	cout<<"5. Set environment variables."<<endl;

	string project_user = project + string(".user");

	if (!file_exists(project_user.c_str()))
	{
		create_vs_user_file(project_user);
	}

	result = doc.load_file(project_user.c_str());
	cout<<result.description()<<endl;
	//check if it is exe

	pugi::xml_node node = doc.child("Project");

	string dir = openvibe_base + string("/scripts");
	_chdir(dir.c_str());


	string s = openvibe_base + string("/scripts/win32-init_env_command.cmd>NUL & set");
	string with_openvibe_vars=exec(_strdup(s.c_str()));

	pugi::xml_node property_group1 = node.append_child("PropertyGroup");
	pugi::xml_node env = property_group1.append_child("LocalDebuggerEnvironment");
    env.append_child(pugi::node_pcdata).set_value(with_openvibe_vars.c_str());

	pugi::xml_node property_group2 = node.append_child("PropertyGroup");
	pugi::xml_node cmd = property_group2.append_child("LocalDebuggerCommand");
	cmd.append_child(pugi::node_pcdata).set_value((openvibe_base+"/dist/bin/"+executable).c_str());

	pugi::xml_node property_group3 = node.append_child("PropertyGroup");
	pugi::xml_node folder = property_group3.append_child("LocalDebuggerWorkingDirectory");
    folder.append_child(pugi::node_pcdata).set_value((openvibe_base+"/dist/bin/").c_str());
	
	doc.save_file(project_user.c_str());
	
	cout<<"Done."<<endl;

	cout<<"TODO:"<<endl;
	cout<<"1) You need to disable the projects you do not need to rebuild in VS->Solution or Project properties->Confugration Manager"<<endl;
	cout<<"2) Set-up your start-up project"<<endl;

	return 0;
}
