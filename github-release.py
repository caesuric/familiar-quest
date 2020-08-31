import requests
import json

def main():
    token = ''
    with open('./Personal Notes/app-token.txt', 'r') as app_token_file:
        token = app_token_file.read()
    auth = ('caesuric', token)
    create_release(auth)
    release_url = get_release_url(auth)
    upload_releases(release_url, auth)

def create_release(auth):
    url = 'https://api.github.com/repos/caesuric/familiar-quest/releases'
    tag_name = 'v'
    with open('./version.txt', 'r') as version_file:
        tag_name += version_file.read()
    name = ''
    with open('./version-name.txt', 'r') as version_name_file:
        name = version_name_file.read()
    description = ''
    with open('./version-description.MD', 'r') as version_description_file:
        description = version_description_file.read()
    params = {
        'tag_name': tag_name,
        'name': name,
        'body': description,
        'draft': False,
        'prerelease': True
    }
    response = requests.post(url, json=params, auth=auth)
    print('created release')
    print(response.status_code)

def get_release_url(auth):
    tag_name = 'v'
    with open('./version.txt', 'r') as version_file:
        tag_name += version_file.read()
    url = f'https://api.github.com/repos/caesuric/familiar-quest/releases/tags/{tag_name}'
    response = requests.get(url, auth=auth)
    json_response = json.loads(response.text)
    release_url = json_response['upload_url']
    release_url = release_url.replace("{?name,label}", "")
    return release_url

def upload_releases(release_url, auth):
    url = release_url
    version = 'v'
    with open('./version.txt', 'r') as version_file:
        version += version_file.read()
    windows_params = {
        'content-type': 'application/zip',
        'name': f'FamiliarQuestWindows.{version}.zip',
        'label': f'Windows release {version} for Familiar Quest'
    }
    linux_params = {
        'content-type': 'application/zip',
        'name': f'FamiliarQuestLinux.{version}.zip',
        'label': f'Linux release {version} for Familiar Quest'
    }
    mac_params = {
        'content-type': 'application/zip',
        'name': f'FamiliarQuestMac.{version}.zip',
        'label': f'Mac release {version} for Familiar Quest'
    }
    with open(f'./Builds/{windows_params["name"]}', 'rb') as windows_release_file:
        print('opened windows release')
        response = requests.post(url+f'?name={windows_params["name"]}', headers=windows_params, data=windows_release_file, auth=auth)
        print('uploaded windows release')
        print(response.status_code)
    with open(f'./Builds/{linux_params["name"]}', 'rb') as linux_release_file:
        print('opened linux release')
        response = requests.post(url+f'?name={linux_params["name"]}', headers=linux_params, data=linux_release_file, auth=auth)
        print('uploaded linux release')
        print(response.status_code)
    with open(f'./Builds/{mac_params["name"]}', 'rb') as mac_release_file:
        print('opened mac release')
        response = requests.post(url+f'?name={mac_params["name"]}', headers=mac_params, data=mac_release_file, auth=auth)
        print('uploaded mac release')
        print(response.status_code)

if __name__=='__main__':
    main()
