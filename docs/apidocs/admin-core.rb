#
# 
#
# Generated by <a href="http://enunciate.webcohesion.com">Enunciate</a>.
#
require 'json'

# adding necessary json serialization methods to standard classes.
class Object
  def to_jaxb_json_hash
    return self
  end
  def self.from_json o
    return o
  end
end

class String
  def self.from_json o
    return o
  end
end

class Boolean
  def self.from_json o
    return o
  end
end

class Numeric
  def self.from_json o
    return o
  end
end

class Time
  #json time is represented as number of milliseconds since epoch
  def to_jaxb_json_hash
    return (to_i * 1000) + (usec / 1000)
  end
  def self.from_json o
    if o.nil?
      return nil
    else
      return Time.at(o / 1000, (o % 1000) * 1000)
    end
  end
end

class Array
  def to_jaxb_json_hash
    a = Array.new
    each { | _item | a.push _item.to_jaxb_json_hash }
    return a
  end
end

class Hash
  def to_jaxb_json_hash
    h = Hash.new
    each { | _key, _value | h[_key.to_jaxb_json_hash] = _value.to_jaxb_json_hash }
    return h
  end
end


module Springfox

module Documentation

module Swagger

module Web

  # (no documentation provided)
  class UiConfiguration 

    # (no documentation provided)
    attr_accessor :docExpansion
    # (no documentation provided)
    attr_accessor :apiSorter
    # (no documentation provided)
    attr_accessor :enableJsonEditor
    # (no documentation provided)
    attr_accessor :defaultModelRendering
    # (no documentation provided)
    attr_accessor :validatorUrl
    # (no documentation provided)
    attr_accessor :showRequestHeaders

    # the json hash for this UiConfiguration
    def to_jaxb_json_hash
      _h = {}
      _h['docExpansion'] = docExpansion.to_jaxb_json_hash unless docExpansion.nil?
      _h['apisSorter'] = apiSorter.to_jaxb_json_hash unless apiSorter.nil?
      _h['jsonEditor'] = enableJsonEditor.to_jaxb_json_hash unless enableJsonEditor.nil?
      _h['defaultModelRendering'] = defaultModelRendering.to_jaxb_json_hash unless defaultModelRendering.nil?
      _h['validatorUrl'] = validatorUrl.to_jaxb_json_hash unless validatorUrl.nil?
      _h['showRequestHeaders'] = showRequestHeaders.to_jaxb_json_hash unless showRequestHeaders.nil?
      return _h
    end

    # the json (string form) for this UiConfiguration
    def to_json
      to_jaxb_json_hash.to_json
    end

    #initializes this UiConfiguration with a json hash
    def init_jaxb_json_hash(_o)
      @docExpansion = String.from_json(_o['docExpansion']) unless _o['docExpansion'].nil?
      @apiSorter = String.from_json(_o['apisSorter']) unless _o['apisSorter'].nil?
      @enableJsonEditor = Boolean.from_json(_o['jsonEditor']) unless _o['jsonEditor'].nil?
      @defaultModelRendering = String.from_json(_o['defaultModelRendering']) unless _o['defaultModelRendering'].nil?
      @validatorUrl = String.from_json(_o['validatorUrl']) unless _o['validatorUrl'].nil?
      @showRequestHeaders = Boolean.from_json(_o['showRequestHeaders']) unless _o['showRequestHeaders'].nil?
    end

    # constructs a UiConfiguration from a (parsed) JSON hash
    def self.from_json(o)
      if o.nil?
        return nil
      else
        inst = new
        inst.init_jaxb_json_hash o
        return inst
      end
    end
  end

end

end

end

end

module Springfox

module Documentation

module Swagger

module Web

  # (no documentation provided)
  class SecurityConfiguration 

    # (no documentation provided)
    attr_accessor :realm
    # (no documentation provided)
    attr_accessor :apiKeyValue
    # (no documentation provided)
    attr_accessor :appName
    # (no documentation provided)
    attr_accessor :clientId
    # (no documentation provided)
    attr_accessor :clientSecret
    # (no documentation provided)
    attr_accessor :apiKeyName
    # (no documentation provided)
    attr_accessor :apiKeyVehicle

    # the json hash for this SecurityConfiguration
    def to_jaxb_json_hash
      _h = {}
      _h['realm'] = realm.to_jaxb_json_hash unless realm.nil?
      _h['apiKey'] = apiKeyValue.to_jaxb_json_hash unless apiKeyValue.nil?
      _h['appName'] = appName.to_jaxb_json_hash unless appName.nil?
      _h['clientId'] = clientId.to_jaxb_json_hash unless clientId.nil?
      _h['clientSecret'] = clientSecret.to_jaxb_json_hash unless clientSecret.nil?
      _h['apiKeyName'] = apiKeyName.to_jaxb_json_hash unless apiKeyName.nil?
      _h['apiKeyVehicle'] = apiKeyVehicle.to_jaxb_json_hash unless apiKeyVehicle.nil?
      return _h
    end

    # the json (string form) for this SecurityConfiguration
    def to_json
      to_jaxb_json_hash.to_json
    end

    #initializes this SecurityConfiguration with a json hash
    def init_jaxb_json_hash(_o)
      @realm = String.from_json(_o['realm']) unless _o['realm'].nil?
      @apiKeyValue = String.from_json(_o['apiKey']) unless _o['apiKey'].nil?
      @appName = String.from_json(_o['appName']) unless _o['appName'].nil?
      @clientId = String.from_json(_o['clientId']) unless _o['clientId'].nil?
      @clientSecret = String.from_json(_o['clientSecret']) unless _o['clientSecret'].nil?
      @apiKeyName = String.from_json(_o['apiKeyName']) unless _o['apiKeyName'].nil?
      @apiKeyVehicle = String.from_json(_o['apiKeyVehicle']) unless _o['apiKeyVehicle'].nil?
    end

    # constructs a SecurityConfiguration from a (parsed) JSON hash
    def self.from_json(o)
      if o.nil?
        return nil
      else
        inst = new
        inst.init_jaxb_json_hash o
        return inst
      end
    end
  end

end

end

end

end

module Springfox

module Documentation

module Swagger

module Web

  # (no documentation provided)
  class SwaggerResource 

    # (no documentation provided)
    attr_accessor :location
    # (no documentation provided)
    attr_accessor :name
    # (no documentation provided)
    attr_accessor :swaggerVersion

    # the json hash for this SwaggerResource
    def to_jaxb_json_hash
      _h = {}
      _h['location'] = location.to_jaxb_json_hash unless location.nil?
      _h['name'] = name.to_jaxb_json_hash unless name.nil?
      _h['swaggerVersion'] = swaggerVersion.to_jaxb_json_hash unless swaggerVersion.nil?
      return _h
    end

    # the json (string form) for this SwaggerResource
    def to_json
      to_jaxb_json_hash.to_json
    end

    #initializes this SwaggerResource with a json hash
    def init_jaxb_json_hash(_o)
      @location = String.from_json(_o['location']) unless _o['location'].nil?
      @name = String.from_json(_o['name']) unless _o['name'].nil?
      @swaggerVersion = String.from_json(_o['swaggerVersion']) unless _o['swaggerVersion'].nil?
    end

    # constructs a SwaggerResource from a (parsed) JSON hash
    def self.from_json(o)
      if o.nil?
        return nil
      else
        inst = new
        inst.init_jaxb_json_hash o
        return inst
      end
    end
  end

end

end

end

end

module Springfox

module Documentation

module Spring

module Web

module Json

  # (no documentation provided)
  class Json 


    # the json hash for this Json
    def to_jaxb_json_hash
      _h = {}
      return _h
    end

    # the json (string form) for this Json
    def to_json
      to_jaxb_json_hash.to_json
    end

    #initializes this Json with a json hash
    def init_jaxb_json_hash(_o)
    end

    # constructs a Json from a (parsed) JSON hash
    def self.from_json(o)
      if o.nil?
        return nil
      else
        inst = new
        inst.init_jaxb_json_hash o
        return inst
      end
    end
  end

end

end

end

end

end
